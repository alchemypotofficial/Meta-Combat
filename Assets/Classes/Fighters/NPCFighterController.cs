using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFighterController : FighterController
{
    [HideInInspector] public NPCIdleState npcIdleState;
    [HideInInspector] public NPCWalkLeftState npcWalkLeftState;
    [HideInInspector] public NPCWalkRightState npcWalkRightState;
    [HideInInspector] public NPCJumpUpState npcJumpUpState;
    [HideInInspector] public NPCJumpLeftState npcJumpLeftState;
    [HideInInspector] public NPCJumpRightState npcJumpRightState;
    [HideInInspector] public NPCCrouchState npcCrouchState;
    [HideInInspector] public NPCLightAttackState npcLightAttackState;
    [HideInInspector] public NPCMediumAttackState npcMediumAttackState;
    [HideInInspector] public NPCHeavyAttackState npcHeavyAttackState;
    [HideInInspector] public NPCKnockDownState npcKnockDownState;
    [HideInInspector] public NPCHurtLightState npcHurtLightState;
    [HideInInspector] public NPCDeathState npcDeathState;

    [SerializeField] protected int npcLevel = 1;

    private float startTime = 0f;

    protected override void Update()
    {
        base.Update();

        if (npcLevel < 1) { npcLevel = 1; }
        if (npcLevel > 15) { npcLevel = 15; }
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        npcIdleState = new NPCIdleState(this, stateMachine);
        npcWalkLeftState = new NPCWalkLeftState(this, stateMachine);
        npcWalkRightState = new NPCWalkRightState(this, stateMachine);
        npcJumpUpState = new NPCJumpUpState(this, stateMachine);
        npcJumpLeftState = new NPCJumpLeftState(this, stateMachine);
        npcJumpRightState = new NPCJumpRightState(this, stateMachine);
        npcCrouchState = new NPCCrouchState(this, stateMachine);
        npcLightAttackState = new NPCLightAttackState(this, stateMachine);
        npcMediumAttackState = new NPCMediumAttackState(this, stateMachine);
        npcHeavyAttackState = new NPCHeavyAttackState(this, stateMachine);
        npcKnockDownState = new NPCKnockDownState(this, stateMachine);
        npcHurtLightState = new NPCHurtLightState(this, stateMachine);
        npcDeathState = new NPCDeathState(this, stateMachine);

        startState = npcIdleState;

        startTime = Time.time;
    }

    public override void SetStartingState()
    {
        stateMachine.ChangeState(npcIdleState);
    }

    public int GetLevelValue()
    {
        return ((15 - npcLevel) + 1) * 2;
    }

    public bool MakeDecision()
    {
        int random = Random.Range(0, 30);

        return random > GetLevelValue() ? true : false;
    }

    public int ChooseDecision(int maxRange)
    {
        int random = Random.Range(0, maxRange);

        return random;
    }

    public bool DecisionDelay()
    {
        float timeOffset = ((float)GetLevelValue() * 0.005f) + 0.1f;

        if (Time.time > startTime + timeOffset)
        {
            startTime = Time.time;

            return true;
        }

        return false;
    }
}
