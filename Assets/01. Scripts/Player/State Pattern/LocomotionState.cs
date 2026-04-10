public class LocomotionState : IPlayerState
{
    private readonly PlayerMovement player;

    private readonly PlayerStateMachine stateMachine;

    public LocomotionState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;

        this.stateMachine = stateMachine;
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }

    public void HandleInput()
    {
        if (player.IsGrounded == false)
        {
            player.ChangeToAirborne();

            return;
        }

        if (player.IsMoving == false)
        {
            player.ChangeToIdle();
        }
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {
        player.Move();

        player.Jump();
    }
}

