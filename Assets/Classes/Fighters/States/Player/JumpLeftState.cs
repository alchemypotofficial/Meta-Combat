using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLeftState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public JumpLeftState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.isJumping = true;
        fighterController.StopWalking();

        fighterController.JumpLeft();
        animator.StartAnimation("Fighter_Jump");

        fighterController.SetHurtBox(HitBox.clear);
    }

    public void ExecuteLogic()
    {
        if (fighterController.IsOnGround())
        {
            fighterController.isJumping = false;
            stateMachine.ChangeState(fighterController.idleState);
        }
    }

    public void ExecutePhysics()
    {

    }

    public void ExecuteHurt(int damage, int stun, Attack attack)
    {

    }

    public void Exit()
    {

    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {
        if (input == InputValue.RIGHT_PRESS && fighterController.IsOnLeftWall() && fighterController.isFacingRight)
        {
            stateMachine.ChangeState(fighterController.jumpRightState);
        }
        else if (input == InputValue.LIGHT_PRESS)
        {
            stateMachine.ChangeState(fighterController.lightAttackState);
        }
        else if (input == InputValue.MEDIUM_PRESS)
        {
            stateMachine.ChangeState(fighterController.mediumAttackState);
        }
        else if (input == InputValue.HEAVY_PRESS)
        {
            stateMachine.ChangeState(fighterController.heavyAttackState);
        }
    }
}