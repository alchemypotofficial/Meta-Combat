using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeathState : IState
{
    private NPCFighterController fighterController;
    private StateMachine stateMachine;
    private AnimationController animator;

    public NPCDeathState(NPCFighterController stateController, StateMachine stateMachine)
    {
        this.fighterController = stateController;
        this.stateMachine = stateMachine;
        this.animator = stateController.GetAnimator();
    }

    public void Enter(IState lastState)
    {
        animator.StartAnimation("Fighter_Death");
        fighterController.KnockDown();

        fighterController.SetHurtBox(HitBox.clear);

        AudioHandler.PlaySound("Fighter_Death");
    }

    public void ExecuteLogic()
    {

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