using System;
using UnityEngine;

public abstract class BaseStateController : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerBaseState previousState;

    public virtual void SwitchState(PlayerBaseState state)
    {
        previousState = currentState;
        currentState = state;
    }
}
