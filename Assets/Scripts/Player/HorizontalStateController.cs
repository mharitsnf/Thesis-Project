using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HorizontalStateController : BaseStateController
{
    // State
    public readonly PlayerIdleState idleState = new();
    public readonly PlayerMoveState moveState = new();

    private void Start()
    {
        currentState = idleState;
        currentState.EnterState();
    }

    private void FixedUpdate()
    {
        // State logic
        currentState.FixedUpdateState();
    }
}