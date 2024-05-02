using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCrouchState : IState
{
    private NPCFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public NPCCrouchState(NPCFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        fighterController.StopWalking();

        fighterController.isCrouching = true;

        if (lastState == fighterController.npcIdleState || lastState == fighterController.npcWalkLeftState || lastState == fighterController.npcWalkRightState)
        {
            animator.StartAnimation("Fighter_Crouch");
        }

        fighterController.SetHurtBox(HitBox.lightCrouch);
    }

    public void ExecuteLogic()
    {
        if (!fighterController.isCrouching)
        {
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
            stateMachine.ChangeState(fighterController.npcKnockDownState);
        }
        else if (attack == Attack.lightNeutral || attack == Attack.lightCrouch || attack == Attack.lightAir)
        {
            fighterController.Damage(damage);
            fighterController.IncreaseStun(stun);

            stateMachine.ChangeState(fighterController.npcHurtLightState);
        }
    }

    public void Exit()
    {

    }

    public void ExecuteInput(InputValue input, bool isHeld)
    {

    }
}