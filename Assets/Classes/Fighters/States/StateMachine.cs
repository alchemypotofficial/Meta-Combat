using UnityEngine;

public class StateMachine
{
    private IState state;
    private IState lastState;

    public StateMachine()
    {
        state = null;
    }

    public void Initialize(IState startState)
    {
        lastState = null;
        state = startState;
        state.Enter(null);
    }

    public void ChangeState(IState newState)
    {
        lastState = state;

        state.Exit();

        state = newState;

        state.Enter(lastState);
    }

    public IState PeekCurrentState()
    {
        return state;
    }

    public IState PeekLastState()
    {
        return lastState;
    }
}
