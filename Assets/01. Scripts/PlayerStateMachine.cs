public class PlayerStateMachine
{
    private IPlayerState currentState;

    public void ChangeState(IPlayerState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState?.ExitState();

        currentState = newState;

        currentState?.EnterState();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Tick()
    {
        currentState?.Tick();
    }

    public void FixedTick()
    {
        currentState?.FixedTick();
    }
}

