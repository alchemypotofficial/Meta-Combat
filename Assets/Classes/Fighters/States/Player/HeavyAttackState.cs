using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public HeavyAttackState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        if (lastState == fighterController.crouchState)
        {
            fighterController.StopWalking();

            animator.StartAnimation("Fighter_Heavy_Crouch");

            fighterController.SetHurtBox(HitBox.lightCrouch);
        }
        else if (lastState == fighterController.jumpUpState || lastState == fighterController.jumpLeftState || lastState == fighterController.jumpRightState)
        {
            animator.StartAnimation("Fighter_Heavy_Air");

            fighterController.SetHurtBox(HitBox.heavyAir);
        }
        else
        {
            fighterController.StopWalking();

            animator.StartAnimation("Fighter_Heavy_Neutral");

            fighterController.SetHurtBox(HitBox.clear);
        }

        AudioHandler.PlaySound("Fighter_Heavy_Attack");
    }

    public void ExecuteLogic()
    {
        IState lastState = stateMachine.PeekLastState();

        // Set hitbox for zone.
        if (animator.GetFrame() == 2)
        {
            if (stateMachine.PeekLastState() == fighterController.crouchState)
            {
                fighterController.SetHitBox(HitBox.heavyCrouch);
            }
            else
            {
                fighterController.SetHitBox(HitBox.heavyNeutral);
            }
        }
        else if (animator.GetFrame() == 1)
        {
            if (lastState == fighterController.jumpUpState || lastState == fighterController.jumpLeftState || lastState == fighterController.jumpRightState)
            {
                fighterController.SetHitBox(HitBox.heavyAir);
            }
        }

        // Change state based on previous state.
        if (animator.IsAnimationPlaying() == false)
        {
            if (fighterController.isCrouching == true && (lastState != fighterController.jumpUpState && lastState != fighterController.jumpLeftState && lastState != fighterController.jumpRightState))
            {
                if (lastState != fighterController.crouchState)
                {
                    animator.StartAnimation("Fighter_Crouch");
                }

                stateMachine.ChangeState(fighterController.crouchState);
            }
            else if (lastState != fighterController.jumpUpState && lastState != fighterController.jumpLeftState && lastState != fighterController.jumpRightState)
            {
                stateMachine.ChangeState(fighterController.idleState);
            }
        }

        if (fighterController.IsOnGround() && (lastState == fighterController.jumpUpState || lastState == fighterController.jumpLeftState || lastState == fighterController.jumpRightState))
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
        if (attack == Attack.heavyCrouch)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.deathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.knockDownState);
            }
        }
        else if (attack == Attack.lightNeutral || attack == Attack.lightCrouch || attack == Attack.lightAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.deathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.knockDownState);
            }
        }
        else if (attack == Attack.mediumNeutral || attack == Attack.mediumCrouch || attack == Attack.mediumAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.deathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.knockDownState);
            }
        }
        else if (attack == Attack.heavyNeutral || attack == Attack.heavyAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.deathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.knockDownState);
            }
        }
    }

    public void Exit()
    {
        fighterController.SetHitBox(HitBox.clear);
    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {

    }
}