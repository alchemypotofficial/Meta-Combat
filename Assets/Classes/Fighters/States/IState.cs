
using UnityEngine;

public interface IState
{
    void Enter(IState lastState); // On entry to State

    void ExecuteLogic(); // Update function of Unity

    void ExecutePhysics(); // FixedUpdate function of Unity

    public void ExecuteInput(InputValue input, bool isHeld);

    public void ExecuteHurt(int damage, int stun, Attack attack);

    void Exit(); // On exit of State
}



