using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMediumAttackState : IState
{
    private NPCFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public NPCMediumAttackState(NPCFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        if (lastState == fighterController.npcCrouchState)
        {
            fighterController.StopWalking();

            animator.StartAnimation("Fighter_Medium_Crouch");

            fighterController.SetHurtBox(HitBox.lightCrouch);
        }
        else if (lastState == fighterController.npcJumpUpState || lastState == fighterController.npcJumpLeftState || lastState == fighterController.npcJumpRightState)
        {
            animator.StartAnimation("Fighter_Medium_Air");

            fighterController.SetHurtBox(HitBox.mediumAir);
        }
        else
        {
            fighterController.StopWalking();

            animator.StartAnimation("Fighter_Medium_Neutral");

            fighterController.SetHurtBox(HitBox.clear);
        }

        AudioHandler.PlaySound("Fighter_Medium_Attack");
    }

    public void ExecuteLogic()
    {
        IState lastState = stateMachine.PeekLastState();

        // Set hitbox for zone.
        if (animator.GetFrame() == 2)
        {
            if (stateMachine.PeekLastState() == fighterController.npcCrouchState)
            {
                fighterController.SetHitBox(HitBox.mediumCrouch);
            }
            else
            {
                fighterController.SetHitBox(HitBox.mediumNeutral);
            }
        }
        else if (animator.GetFrame() == 1)
        {
            if (lastState == fighterController.npcJumpUpState || lastState == fighterController.npcJumpLeftState || lastState == fighterController.npcJumpRightState)
            {
                fighterController.SetHitBox(HitBox.mediumAir);
            }
        }

        // Change state based on previous state.
        if (animator.IsAnimationPlaying() == false)
        {
            if (fighterController.isCrouching == true && (lastState != fighterController.npcJumpUpState && lastState != fighterController.npcJumpLeftState && lastState != fighterController.npcJumpRightState))
            {
                if (lastState != fighterController.npcCrouchState)
                {
                    animator.StartAnimation("Fighter_Crouch");
                }

                stateMachine.ChangeState(fighterController.npcCrouchState);
            }
            else if (lastState != fighterController.npcJumpUpState && lastState != fighterController.npcJumpLeftState && lastState != fighterController.npcJumpRightState)
            {
                stateMachine.ChangeState(fighterController.npcIdleState);
            }
        }

        if (fighterController.IsOnGround() && (lastState == fighterController.npcJumpUpState || lastState == fighterController.npcJumpLeftState || lastState == fighterController.npcJumpRightState))
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
        if (attack == Attack.heavyCrouch)
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
        else if (attack == Attack.lightNeutral || attack == Attack.lightCrouch || attack == Attack.lightAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.npcDeathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.npcHurtLightState);
            }
        }
        else if (attack == Attack.mediumNeutral || attack == Attack.mediumCrouch || attack == Attack.mediumAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.npcDeathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.npcHurtLightState);
            }
        }
        else if (attack == Attack.heavyNeutral || attack == Attack.heavyAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            if (fighterController.health == 0)
            {
                stateMachine.ChangeState(fighterController.npcDeathState);
            }
            else
            {
                stateMachine.ChangeState(fighterController.npcHurtLightState);
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
