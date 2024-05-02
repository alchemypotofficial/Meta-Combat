using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MovementController : MonoBehaviour
{
    public Collider2D characterBlocker;
    public GameObject opponent;
    public Transform groundTransform;

    private Rigidbody2D _body;
    private BoxCollider2D _pushBox;
    private SpriteRenderer _spriteRenderer;
    private AnimationController _animController;

    public bool IsMovingRight { get; set; }
    public bool IsMovingLeft { get; set; }
    public bool IsCrouching { get; set; }

    public bool IsJumping { get; set; }

    public bool IsAirbourne { get { return GetIsAirbourne(); } }
    public bool IsFacingRight { get; set; }

    public AnimationController AnimController { get { return _animController; } }

    private void Awake()
    {
        _body = this.GetComponent<Rigidbody2D>();
        _pushBox = this.GetComponent<BoxCollider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _animController = this.GetComponent<AnimationController>();
    }

    private void Start()
    {
        Physics2D.IgnoreCollision(_pushBox, characterBlocker, true);
    }

    private void FixedUpdate()
    {
        UpdateFacing();
        UpdateSpacing();
    }

    public void JumpVertical()
    {
        if (!IsAirbourne)
        {
            SetVertical(1f);
        }
    }

    public void JumpRight()
    {
        if (!IsAirbourne)
        {
            SetVertical(1f);
            SetHorizontal(5);
        }
    }

    public void JumpLeft()
    {
        if (!IsAirbourne)
        {
            SetVertical(1f);
            SetHorizontal(-5);
        }
    }

    public void WalkForward()
    {
        float velocity = 10f;
        SetHorizontal(velocity);

        //if (IsFacingRight)
        //{
        //    SetHorizontal(velocity);
        //}
        //else
        //{
        //    SetHorizontal(-velocity);
        //}
    }

    public void WalkBackward()
    {
        float velocity = 10f;
        SetHorizontal(-velocity);

        //if (IsFacingRight)
        //{
        //    SetHorizontal(-velocity);
        //}
        //else
        //{
        //    SetHorizontal(velocity);
        //}
    }

    public void Crouch()
    {

    }

    public void Stop()
    {
        _body.velocity = Vector2.zero;
    }

    public void StopHorizontal()
    {
        _body.velocity = new Vector2(0, _body.velocity.y);
    }

    public void StopVertical()
    {
        _body.velocity = new Vector2(_body.velocity.x, 0);
    }

    private Vector2 GetVelocity()
    {
        return _body.velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _body.velocity = velocity;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        _body.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void SetHorizontal(float xVelocity)
    {
        SetVelocity(xVelocity, _body.velocity.y);
    }

    private void SetVertical(float yVelocity)
    {
        SetVelocity(_body.velocity.x, yVelocity);
    }

    public void AddHorizontal(float xVelocity)
    {
        AddVelocity(xVelocity, 0);
    }

    public void AddVertical(float yVelocity)
    {
        AddVelocity(0, yVelocity);
    }

    public void AddVelocity(Vector2 velocity)
    {
        AddVelocity(velocity.x, velocity.y);
    }

    public void AddVelocity(float xVelocity, float yVelocity)
    {
        _body.velocity = new Vector2(_body.velocity.x + xVelocity, _body.velocity.y + yVelocity);
    }

    private void ImpartForce(Vector2 force)
    {
        _body.AddForce(force);
    }

    private void ImpartImpulse(Vector2 force)
    {
        _body.AddForce(force, ForceMode2D.Impulse);
    }

    public void FaceRight()
    {
        if (!IsFacingRight)
        {
            IsFacingRight = true;
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void FaceLeft()
    {
        if (IsFacingRight)
        {
            IsFacingRight = false;
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    private void Turn()
    {
        
        IsFacingRight = !IsFacingRight;
    }

    private bool GetIsAirbourne()
    {
        /* Check if player is above some position in world space (defined by the bottom of the player
         * pushbox and the top of the ground surface) to see if airbourne. This simple check is MUCH 
         * faster than doing a raycast or a collision check and thus as long as the ground is always level
         * this trick works. */

        // y value of bottom of pushbox in world space
        float pushBoxBottomY = this.transform.position.y - (_pushBox.size.y / 2) + _pushBox.offset.y;
        float groundSurfaceY = groundTransform.position.y + (groundTransform.localScale.y / 2);

        if (pushBoxBottomY > (groundSurfaceY + (Physics2D.defaultContactOffset * 2)))
        {
            return true;
        }

        return false;
    }

    private void UpdateFacing()
    {
        /* Update the players direction (JUST THE SPRITE) so that it always faces the opponent
         * as long as its grounded. (character sprite doesnt flip while in the air)
         * THIS DOES NOT CHANGE THE PHYSICS DIRECTION ie. "Forward" is NOT changed thus use "left/right" NOT
         * "forward/back". */

        MovementController _opponent_move_contr = opponent.GetComponent<MovementController>();

        // If player is grounded
        if (!IsAirbourne && !_opponent_move_contr.IsAirbourne)
        {
            // If player is on the left of opponent
            if ((opponent.transform.position - this.transform.position).x > 0)
            {
                FaceRight();
            }
            // else player is on the right
            else
            {
                FaceLeft();
            }
        }
    }

    private void UpdateSpacing()
    {
        float playerBeginX = this.transform.position.x + _pushBox.offset.x;
        float playerEndX = this.transform.position.x + _pushBox.offset.x + _pushBox.size.x;
        float playerY = this.transform.position.y + _pushBox.offset.y;

        float opponentBeginX = opponent.transform.position.x + _pushBox.offset.x;
        float opponentEndX = opponent.transform.position.x + _pushBox.offset.x + _pushBox.size.x;
        float opponentY = opponent.transform.position.y + _pushBox.offset.y;

        if ((opponentBeginX >= playerBeginX && opponentBeginX <= playerEndX || opponentEndX >= playerBeginX && opponentEndX <= playerEndX) && opponentY >= playerY)
        {
            if(IsFacingRight)
            {
                SetHorizontal(3);
            }
            else
            {
                SetHorizontal(-3);
            }
        }
        else if ((opponentBeginX >= playerBeginX && opponentBeginX <= playerEndX || opponentEndX >= playerBeginX && opponentEndX <= playerEndX) && opponentY <= playerY)
        {
            if (IsFacingRight)
            {
                SetHorizontal(3);
            }
            else
            {
                SetHorizontal(-3);
            }
        }
    }
}
