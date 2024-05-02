using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCJumpRightState : IState
{
    private NPCFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public NPCJumpRightState(NPCFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.isJumping = true;
        fighterController.StopWalking();

        fighterController.JumpRight();
        animator.StartAnimation("Fighter_Jump");

        fighterController.SetHurtBox(HitBox.clear);
    }

    public void ExecuteLogic()
    {
        if (fighterController.IsOnGround())
        {
            fighterController.isJumping = false;
            stateMachine.ChangeState(fighterController.npcIdleState);
        }
    }

    public void ExecutePhysics()
    {

    }

    public void ExecuteHurt(int damage, int stun, Attack attack)
    {
        fighterController.Damage(damage);
        fighterController.IncreaseStun(stun);

        if (fighterController.health == 0)
        {
            stateMachine.ChangeState(fighterController.npcDeathState);
        }
        else
        {
            stateMachine.ChangeState(fighterController.npcKnockDownState);
        }
    }

    public void Exit()
    {

    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {

    }
}