public abstract class StickmanState
{
    protected StickmanBase stateMachine;

    public StickmanState(StickmanBase stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract string GetName();

    public abstract void Execute();

    public abstract void FixedExecute();

    public abstract void ExitState(StickmanState exitTo);
}
