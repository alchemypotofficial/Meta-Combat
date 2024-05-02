using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtLightState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public HurtLightState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.StopWalking();

        if (lastState == fighterController.crouchState)
        {
            animator.StartAnimation("Fighter_Hurt_Light_Crouch");
        }
        else
        {
            animator.StartAnimation("Fighter_Hurt_Light_Neutral");
        }

        fighterController.Slide();

        AudioHandler.PlaySound("Fighter_Hurt_Light");
    }

    public void ExecuteLogic()
    {
        if (animator.GetFrame() == 2)
        {
            if (fighterController.isCrouching)
            {
                stateMachine.ChangeState(fighterController.idleState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.idleState);
            }
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
        animator.StopAnimation();
    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {

    }
}
