using System.Collections;
using UnityEngine;

public class RetireState : IPlayerState
{
    private readonly PlayerMovement player;

    private readonly PlayerStateMachine stateMachine;

    public RetireState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;

        this.stateMachine = stateMachine;
    }

    public void EnterState()
    {
        player.StartCoroutine(RetireRoutine());
    }

    public void ExitState()
    {

    }

    public void FixedTick()
    {

    }

    public void HandleInput()
    {

    }

    public void Tick()
    {

    }

    private IEnumerator RetireRoutine()
    {
        Debug.Log("Player Retire");

        yield return new WaitForSeconds(2f);

        player.ChangeToIdle();
    }
}
