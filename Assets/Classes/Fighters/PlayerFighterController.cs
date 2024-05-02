using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighterController : FighterController
{
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public WalkRightState walkRightState;
    [HideInInspector] public WalkLeftState walkLeftState;
    [HideInInspector] public CrouchState crouchState;
    [HideInInspector] public JumpUpState jumpUpState;
    [HideInInspector] public JumpLeftState jumpLeftState;
    [HideInInspector] public JumpRightState jumpRightState;
    [HideInInspector] public LightAttackState lightAttackState;
    [HideInInspector] public MediumAttackState mediumAttackState;
    [HideInInspector] public HeavyAttackState heavyAttackState;
    [HideInInspector] public HurtLightState hurtLightState;
    [HideInInspector] public KnockDownState knockDownState;
    [HideInInspector] public DeathState deathState;

    protected override void Update()
    {
        base.Update();

        isCrouching = InputHandler.playerControls.Player.Down.IsPressed();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        idleState = new IdleState(this, stateMachine);
        walkRightState = new WalkRightState(this, stateMachine);
        walkLeftState = new WalkLeftState(this, stateMachine);
        crouchState = new CrouchState(this, stateMachine);
        jumpUpState = new JumpUpState(this, stateMachine);
        jumpLeftState = new JumpLeftState(this, stateMachine);
        jumpRightState = new JumpRightState(this, stateMachine);
        lightAttackState = new LightAttackState(this, stateMachine);
        mediumAttackState = new MediumAttackState(this, stateMachine);
        heavyAttackState = new HeavyAttackState(this, stateMachine);
        hurtLightState = new HurtLightState(this, stateMachine);
        knockDownState = new KnockDownState(this, stateMachine);
        deathState = new DeathState(this, stateMachine);

        startState = idleState;
    }

    public override void SetStartingState()
    {
        stateMachine.ChangeState(idleState);
    }
}
