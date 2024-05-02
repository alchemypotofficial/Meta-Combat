using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum HitBox
{
    clear,
    lightNeutral,
    lightCrouch,
    lightAir,
    mediumNeutral,
    mediumCrouch,
    mediumAir,
    heavyNeutral,
    heavyCrouch,
    heavyAir
}

public enum Attack
{
    lightNeutral,
    lightCrouch,
    lightAir,
    mediumNeutral,
    mediumCrouch,
    mediumAir,
    heavyNeutral,
    heavyCrouch,
    heavyAir
}

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
public class FighterController : StateController
{
    [Header("Fighter Settings")]
    [SerializeField] private AnimationController animator;
    [SerializeField] private BoxCollider2D hurtBox;

    [Header("Hit Boxes")]
    [SerializeField] public PolygonCollider2D lightNeutral;
    [SerializeField] public PolygonCollider2D lightCrouch;
    [SerializeField] public PolygonCollider2D lightAir;
    [SerializeField] public PolygonCollider2D mediumNeutral;
    [SerializeField] public PolygonCollider2D mediumCrouch;
    [SerializeField] public PolygonCollider2D mediumAir;
    [SerializeField] public PolygonCollider2D heavyNeutral;
    [SerializeField] public PolygonCollider2D heavyCrouch;
    [SerializeField] public PolygonCollider2D heavyAir;

    public int health = 100;
    public int maxHealth = 100;
    public int stun = 0;
    public int stunCapacity = 100;
    public bool isStunned = false;
    public bool isDead = false;
    public bool isWalkingLeft = false;
    public bool isWalkingRight = false;
    public bool isJumping = false;
    public bool isCrouching = false;
    public bool isFacingRight = true;
    public bool isControllable = false;
    public bool canDamage = false;

    private InputValue leftRightSOCD;
    public bool isKnockedDown;
    private bool leftPressed;
    private bool rightPressed;

    private PolygonCollider2D currentCollider;
    [SerializeField] private HitBox currentHitBox = HitBox.clear;
    [SerializeField] private string currentState = "";

    private Vector3 velocity = Vector3.zero;

    protected override void InitializeStates()
    {

    }

    public virtual void SetStartingState()
    {

    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(ReduceStun());

        if (animator == null)
        {
            Log.Alert("Fighter animator is null! Please grab an animator for the fighter.");
        }

        currentCollider = gameObject.AddComponent<PolygonCollider2D>();
        currentCollider.isTrigger = true;
        currentCollider.enabled = false;

        currentCollider.pathCount = 0;
    }

    protected override void Update()
    {
        base.Update();

        currentState = stateMachine.PeekCurrentState().ToString();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (transform.position.y > -2.5f)
        {
            velocity.y += -15f * Time.fixedDeltaTime;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -2.5f);

            if (isKnockedDown)
            {
                velocity.y = -velocity.y;
                velocity *= 0.3f;

                if (velocity.y > -0.05f && velocity.y < 0.1f)
                {
                    isKnockedDown = false;
                    velocity.y = 0;
                }    
            }
            else
            {
                velocity.y = 0;
            }
        }

        transform.position += velocity * Time.fixedDeltaTime;

        if (transform.position.x < CameraHandler.GetLeftBorder() + 0.5f)
        {
            transform.position = new Vector3(CameraHandler.GetLeftBorder() + 0.5f, transform.position.y);
        }
        else if (transform.position.x > CameraHandler.GetRightBorder() - 0.5f)
        {
            transform.position = new Vector3(CameraHandler.GetRightBorder() - 0.5f, transform.position.y);
        }
    }

    private void OnEnable()
    {
        InputHandler.OnInputEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputHandler.OnInputEvent -= OnInputEvent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FighterController otherFighter = other.gameObject.GetComponent<FighterController>();

        if (canDamage && otherFighter != null && otherFighter != this)
        {
            otherFighter.canDamage = false;

            Log.Alert(other.name + " Fighter has been hit.");

            switch (currentHitBox)
            {
                case HitBox.lightNeutral:
                    otherFighter.OnHitEvent(5, 5, Attack.lightNeutral);
                    break;
                case HitBox.lightCrouch:
                    otherFighter.OnHitEvent(5, 5, Attack.lightCrouch);
                    break;
                case HitBox.lightAir:
                    otherFighter.OnHitEvent(5, 5, Attack.lightAir);
                    break;
                case HitBox.mediumNeutral:
                    otherFighter.OnHitEvent(9, 9, Attack.mediumNeutral);
                    break;
                case HitBox.mediumCrouch:
                    otherFighter.OnHitEvent(9, 9, Attack.mediumCrouch);
                    break;
                case HitBox.mediumAir:
                    otherFighter.OnHitEvent(9, 9, Attack.mediumAir);
                    break;
                case HitBox.heavyNeutral:
                    otherFighter.OnHitEvent(15, 15, Attack.heavyNeutral);
                    break;
                case HitBox.heavyCrouch:
                    otherFighter.OnHitEvent(15, 25, Attack.heavyCrouch);
                    break;
                case HitBox.heavyAir:
                    otherFighter.OnHitEvent(15, 15, Attack.heavyAir);
                    break;
            }
        }
    }

    private void OnInputEvent(InputValue input, bool isHeld)
    {
        if (isControllable)
        {
            if (input == InputValue.LEFT_PRESS || input == InputValue.RIGHT_PRESS)
            {
                ExecuteLeftRightSOCD(input, isHeld);
            }
            else
            {
                if (input == InputValue.LEFT_RELEASE)
                {
                    leftPressed = false;
                }
                else if (input == InputValue.RIGHT_RELEASE)
                {
                    rightPressed = false;
                }

                stateMachine.PeekCurrentState().ExecuteInput(input, isHeld);
            }
        }
    }

    private void OnHitEvent(int damage, int stun, Attack attack)
    {
        stateMachine.PeekCurrentState().ExecuteHurt(damage, stun, attack);
    }
    
    private void ExecuteLeftRightSOCD(InputValue input, bool isHeld)
    {
        if (input == InputValue.LEFT_PRESS && !leftPressed)
        {
            leftPressed = true;
            leftRightSOCD = InputValue.LEFT_PRESS;
        }
        else if (input == InputValue.LEFT_PRESS && !rightPressed)
        {
            leftRightSOCD = InputValue.LEFT_PRESS;
        }

        if (input == InputValue.RIGHT_PRESS && !rightPressed)
        {
            rightPressed = true;
            leftRightSOCD = InputValue.RIGHT_PRESS;
        }
        else if (input == InputValue.RIGHT_PRESS && !leftPressed)
        {
            leftRightSOCD = InputValue.RIGHT_PRESS;
        }

        stateMachine.PeekCurrentState().ExecuteInput(leftRightSOCD, isHeld);
    }

    public void WalkRight()
    {
        velocity.x = 3.25f;
    }

    public void WalkLeft()
    {
        velocity.x = -3.25f;
    }

    public void StopWalking()
    {
        velocity.x = 0f;
    }

    public void JumpUp()
    {
        velocity.y = 10;

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void JumpLeft()
    {
        velocity.y = 10;
        velocity.x = -5;

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void JumpRight()
    {
        velocity.y = 10;
        velocity.x = 5;

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void KnockDown()
    {
        isKnockedDown = true;

        if (isFacingRight)
        {
            velocity.x = -2f;
        }
        else
        {
            velocity.x = 2f;
        }

        velocity.y = 8f;

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void Slide()
    {
        if (isFacingRight)
        {
            velocity.x = -5f;
        }
        else
        {
            velocity.x = 5f;
        }

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public bool IsOnGround()
    {
        if (transform.position.y <= -2.5f)
        {
            return true;
        }

        return false;
    }

    public bool IsOnLeftWall()
    {
        if (!IsOnGround() && transform.position.x == CameraHandler.GetLeftBorder() + 0.5f && transform.position.y >= 0.4f)
        {
            Log.Alert("Is on left wall.");

            return true;
        }

        return false;
    }

    public bool IsOnRightWall()
    {
        if (!IsOnGround() && transform.position.x == CameraHandler.GetRightBorder() - 0.5f && transform.position.y >= 0.4f)
        {
            return true;
        }

        return false;
    }

    public void SetHitBox(HitBox value)
    {
        if (currentCollider != null)
        {
            if (value != HitBox.clear)
            {
                PolygonCollider2D newCollider = null;

                switch (value)
                {
                    case HitBox.lightNeutral:
                        newCollider = lightNeutral;
                        break;
                    case HitBox.lightCrouch:
                        newCollider = lightCrouch;
                        break;
                    case HitBox.lightAir:
                        newCollider = lightAir;
                        break;
                    case HitBox.mediumNeutral:
                        newCollider = mediumNeutral;
                        break;
                    case HitBox.mediumCrouch:
                        newCollider = mediumCrouch;
                        break;
                    case HitBox.mediumAir:
                        newCollider = mediumAir;
                        break;
                    case HitBox.heavyNeutral:
                        newCollider = heavyNeutral;
                        break;
                    case HitBox.heavyCrouch:
                        newCollider = mediumCrouch;
                        break;
                    case HitBox.heavyAir:
                        newCollider = heavyAir;
                        break;
                }

                if (newCollider != null)
                {
                    currentCollider.SetPath(0, newCollider.GetPath(0));
                    currentCollider.offset = newCollider.offset;
                    currentCollider.enabled = true;

                    currentHitBox = value;

                    canDamage = true;
                }
            }
            else
            {
                currentCollider.pathCount = 0;
                currentHitBox = value;
                currentCollider.enabled = false;

                canDamage = false;
            }
        }
    }

    public void SetHurtBox(HitBox value)
    {
        switch (value)
        {
            case HitBox.lightCrouch:
                hurtBox.offset = new Vector2(0f, -0.7f);
                hurtBox.size = new Vector2(0.85f, 2f);
                break;
            case HitBox.lightAir:
                hurtBox.offset = new Vector2(-0.16f, -0.225f);
                hurtBox.size = new Vector2(1.6f, 2f);
                break;
            case HitBox.mediumAir:
                hurtBox.offset = new Vector2(-0.07f, -0.07f);
                hurtBox.size = new Vector2(1.75f, 2.25f);
                break;
            case HitBox.heavyAir:
                hurtBox.offset = new Vector2(-0.45f, -0.625f);
                hurtBox.size = new Vector2(1.75f, 2f);
                break;
            default:
                hurtBox.offset = new Vector2(0f, -0.2f);
                hurtBox.size = new Vector2(0.85f, 3f);
                break;
        }
    }

    public void IncreaseStun(int amount)
    {
        stun += amount;
        if (stun >= 100) { isStunned = true; }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0) { health = 0; isDead = true; }
    }

    public AnimationController GetAnimator()
    {
        return animator;
    }

    private IEnumerator ReduceStun()
    {
        while (true)
        {
            if (stun > 0) { stun -= 10; }
            if (stun < 0) { stun = 0; }
            
            yield return new WaitForSeconds(1f);
        }
    }
}
