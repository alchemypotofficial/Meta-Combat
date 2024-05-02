using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    public StateMachine stateMachine = null;
    public IState startState = null;

    protected abstract void InitializeStates();

    protected virtual void Start()
    {
        stateMachine = new StateMachine();

        InitializeStates();
        InitializeStateMachine();
    }

    protected virtual void Update()
    {
        if (stateMachine.PeekCurrentState() != null)
        {
            stateMachine.PeekCurrentState().ExecuteLogic();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (stateMachine.PeekCurrentState() != null)
        {
            stateMachine.PeekCurrentState().ExecutePhysics();
        }
    }
    
    private void InitializeStateMachine()
    {
        stateMachine.Initialize(startState);
    }
}
