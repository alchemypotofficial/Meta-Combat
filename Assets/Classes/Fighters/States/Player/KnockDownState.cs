using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    private bool isGettingUp;

    public KnockDownState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        animator.StartAnimation("Fighter_KnockDown");
        fighterController.KnockDown();

        fighterController.SetHurtBox(HitBox.clear);

        isGettingUp = false;
    }

    public void ExecuteLogic()
    {
        if (fighterController.isKnockedDown == true && fighterController.IsOnGround() && animator.GetCurrentAnimation() != "Fighter_Lying")
        {
            animator.StartAnimation("Fighter_Lying");
        }

        if (fighterController.isKnockedDown == false)
        {
            if (isGettingUp == false)
            {
                isGettingUp = true;
                animator.StartAnimation("Fighter_GetUp");
            }
            else
            {
                if (animator.IsAnimationPlaying() == false)
                {
                    isGettingUp = false;
                    stateMachine.ChangeState(fighterController.idleState);
                }
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

    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {

    }
}