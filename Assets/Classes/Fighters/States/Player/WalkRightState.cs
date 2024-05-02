using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRightState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public WalkRightState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        animator.StartAnimation("Fighter_Walk", true);

        fighterController.SetHurtBox(HitBox.clear);
    }

    public void ExecuteLogic()
    {

    }

    public void ExecutePhysics()
    {
        fighterController.WalkRight();
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
                stateMachine.ChangeState(fighterController.hurtLightState);
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
                stateMachine.ChangeState(fighterController.hurtLightState);
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
                stateMachine.ChangeState(fighterController.hurtLightState);
            }
        }
    }

    public void Exit()
    {
        animator.StopAnimation();
    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {
        if (input == InputValue.LEFT_PRESS)
        {
            stateMachine.ChangeState(fighterController.walkLeftState);
        }
        else if (input == InputValue.RIGHT_RELEASE)
        {
            stateMachine.ChangeState(fighterController.idleState);
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
        else if (input == InputValue.UP_PRESS)
        {
            stateMachine.ChangeState(fighterController.jumpRightState);
        }
        else if (input == InputValue.DOWN_PRESS)
        {
            stateMachine.ChangeState(fighterController.crouchState);
        }
    }
}
