using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : IState
{
    private NPCFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public NPCIdleState(NPCFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.StopWalking();
        animator.StartAnimation("Fighter_Idle", true);

        fighterController.SetHurtBox(HitBox.clear);
    }

    public void ExecuteLogic()
    {
        if (fighterController.isControllable)
        {
            if (fighterController.DecisionDelay())
            {
                if (Vector3.Distance(fighterController.transform.position, GameHandler.GetLeftFighter().transform.position) <= 2f)
                {
                    // Attack if close enough to player.
                    if (fighterController.MakeDecision())
                    {
                        int choice = fighterController.ChooseDecision(10);

                        if (choice < 4)
                        {
                            stateMachine.ChangeState(fighterController.npcLightAttackState);
                        }
                        else if (choice < 8)
                        {
                            stateMachine.ChangeState(fighterController.npcMediumAttackState);
                        }
                        else
                        {
                            stateMachine.ChangeState(fighterController.npcHeavyAttackState);
                        }
                    }
                }
                else if (Vector3.Distance(fighterController.transform.position, GameHandler.GetLeftFighter().transform.position) > 2f)
                {
                    // Move closer to player.
                    if (fighterController.MakeDecision())
                    {
                        if (fighterController.isFacingRight)
                        {
                            stateMachine.ChangeState(fighterController.npcWalkRightState);
                        }
                        else
                        {
                            stateMachine.ChangeState(fighterController.npcWalkLeftState);
                        }
                    }
                }
                else if (Vector3.Distance(fighterController.transform.position, GameHandler.GetLeftFighter().transform.position) > 6f)
                {
                    if (fighterController.MakeDecision())
                    {
                        if (fighterController.isFacingRight)
                        {
                            stateMachine.ChangeState(fighterController.npcJumpRightState);
                        }
                        else
                        {
                            stateMachine.ChangeState(fighterController.npcJumpLeftState);
                        }
                    }
                }
            }
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

    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {
        
    }
}
