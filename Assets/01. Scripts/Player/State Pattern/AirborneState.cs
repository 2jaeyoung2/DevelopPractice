public class AirborneState : IPlayerState
{
    private readonly PlayerMovement player;

    private readonly PlayerStateMachine stateMachine;

    public AirborneState(PlayerMovement player, PlayerStateMachine stateMachine)
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
        // 공중 중에는 입력으로 상태 전환하지 않고, 착지로만 전환
    }

    public void Tick()
    {
        if (player.IsGrounded == true)
        {
            if (player.IsMoving == true)
            {
                player.ChangeToLocomotion();
            }
            else
            {
                player.ChangeToIdle();
            }
        }
    }

    public void FixedTick()
    {
        // 현재 PlayerMovement 구현은 공중 이동 제어도 동일하게 적용됨.
        // (원하면 나중에 AirMove()를 따로 만들어서 공중 제어만 약하게 만들면 됨)
        player.Move();

        player.Jump();
    }
}

