using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public IdleState(PlayerFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.StopWalking();
        animator.StartAnimation("Fighter_Idle", true);

        fighterController.SetHitBox(HitBox.clear);
        fighterController.SetHurtBox(HitBox.clear);
    }

    public void ExecuteLogic()
    {

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
        if (fighterController.isControllable)
        {
            switch (input)
            {
                case InputValue.LEFT_PRESS:
                    stateMachine.ChangeState(fighterController.walkLeftState);
                    break;
                case InputValue.RIGHT_PRESS:
                    stateMachine.ChangeState(fighterController.walkRightState);
                    break;
                case InputValue.UP_PRESS:
                    stateMachine.ChangeState(fighterController.jumpUpState);
                    break;
                case InputValue.DOWN_PRESS:
                    stateMachine.ChangeState(fighterController.crouchState);
                    break;
                case InputValue.LIGHT_PRESS:
                    stateMachine.ChangeState(fighterController.lightAttackState);
                    break;
                case InputValue.MEDIUM_PRESS:
                    stateMachine.ChangeState(fighterController.mediumAttackState);
                    break;
                case InputValue.HEAVY_PRESS:
                    stateMachine.ChangeState(fighterController.heavyAttackState);
                    break;
            }
        }
    }
}