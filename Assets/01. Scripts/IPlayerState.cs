using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void EnterState();

    public void ExitState();

    public void HandleInput();

    public void Tick();

    public void FixedTick();
}
