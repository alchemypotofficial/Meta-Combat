using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
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

    // Opponent data
    public FighterController opponentController;
    public Collider2D opponentHurtBox;

    // Attack colliders
    [Header("Hit Boxes")]
    public PolygonCollider2D lightNeutral;
    public PolygonCollider2D lightCrouch;
    public PolygonCollider2D lightAir;
    public PolygonCollider2D mediumNeutral;
    public PolygonCollider2D mediumCrouch;
    public PolygonCollider2D mediumAir;
    public PolygonCollider2D heavyNeutral;
    public PolygonCollider2D heavyCrouch;
    public PolygonCollider2D heavyAir;

    // Used for organization
    private PolygonCollider2D[] colliders;
    private PolygonCollider2D currentCollider;
    
    private FighterController fighterController;

    [SerializeField] private HitBox currentHitBox = HitBox.clear;

    void Start()
    {
        fighterController = gameObject.GetComponent<FighterController>();

        colliders = new PolygonCollider2D[] { lightNeutral, lightCrouch, lightAir, mediumNeutral, mediumCrouch, mediumAir, heavyNeutral, heavyCrouch, heavyAir };

        currentCollider = gameObject.AddComponent<PolygonCollider2D>();
        currentCollider.isTrigger = true;

        currentCollider.pathCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == opponentHurtBox)
        {
        }
    }

    public void SetHitBox(HitBox value)
    {
        if (currentCollider != null)
        {
            if (value != HitBox.clear)
            {
                if (colliders[(int)value - 1] == null) { return; }

                currentCollider.SetPath(0, colliders[(int)value - 1].GetPath(0));
                currentCollider.offset = colliders[(int)value - 1].offset;

                currentHitBox = value;
            }
            else
            {
                currentCollider.pathCount = 0;
                currentHitBox = value;
            }
        }
    }
}
