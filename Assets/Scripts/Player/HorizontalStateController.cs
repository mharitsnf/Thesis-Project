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
        AdjustCrouching();
        currentState.FixedUpdateState();
    }
    
    private void AdjustCrouching()
    {
        if (!PlayerData.Instance.IsCrouching) return;
        if (!PlayerData.Instance.isGrounded) return;
        if (!PlayerData.Instance.groundInfo.rigidbody) return;

        PlayerData.Instance.rigidBody.velocity = PlayerData.Instance.groundInfo.rigidbody.velocity;
        // PlayerData.Instance.rigidBody.AddForce(Physics.gravity * 5f, ForceMode.Acceleration);
    }
}