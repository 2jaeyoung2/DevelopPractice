public class IdleState : IPlayerState
{
    private readonly PlayerMovement player;

    private readonly PlayerStateMachine stateMachine;

    public IdleState(PlayerMovement player, PlayerStateMachine stateMachine)
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

        if (player.IsMoving == true)
        {
            player.ChangeToLocomotion();
        }
    }

    public void Tick()
    {
    }

    public void FixedTick()
    {
        player.StopGroundDrift();

        // 점프 버퍼 / 코요테 점프는 상태와 무관하게 항상 체크
        player.Jump();
    }
}

