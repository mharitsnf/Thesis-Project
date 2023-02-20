using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerHorizontalStateManager : PlayerBaseManager
{
    [Header("Movement And Rotation")] 
    public float rotationSpeed = 10; 
    public float acceleration = 100;
    public float maxSpeed = 10;

    // State
    public readonly PlayerIdleState idleState = new();
    public readonly PlayerMoveState moveState = new();

    private void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        // State logic
        currentState.FixedUpdateState(this);
    }
    
    public override void SwitchState(PlayerBaseState state)
    {
        base.SwitchState(state);
        currentState.EnterState(this);
    }
}