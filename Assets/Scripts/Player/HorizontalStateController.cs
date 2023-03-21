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
        AdjustCrouching();
    }

    private void AdjustCrouching()
    {
        if (!PlayerData.Instance.IsCrouching) return;
        if (!PlayerData.Instance.isGrounded) return;
        if (!PlayerData.Instance.groundInfo.rigidbody) return;

        Rigidbody mechanicRb = PlayerData.Instance.groundInfo.rigidbody;
        Vector3 velocity = PlayerData.Instance.rigidBody.velocity;

        velocity = Vector3.Lerp(velocity, mechanicRb.velocity, PlayerData.Instance.crouchingSmoothness * Time.fixedDeltaTime);
        PlayerData.Instance.rigidBody.velocity = velocity;
    }
}