using System;
using UnityEngine;

public abstract class BaseStateController : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerBaseState previousState;

    public void SwitchState(PlayerBaseState state)
    {
        currentState.ExitState();
        previousState = currentState;
        currentState = state;
        currentState.EnterState();
    }
}
