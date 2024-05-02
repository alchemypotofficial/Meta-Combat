using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public CrouchState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.StopWalking();

        fighterController.isCrouching = true;

        if (lastState == fighterController.idleState || lastState == fighterController.walkLeftState || lastState == fighterController.walkRightState)
        {
            animator.StartAnimation("Fighter_Crouch");
        }

        fighterController.SetHurtBox(HitBox.lightCrouch);
    }

    public void ExecuteLogic()
    {
        if (!fighterController.isCrouching)
        {
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
        if (input == InputValue.LIGHT_PRESS)
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