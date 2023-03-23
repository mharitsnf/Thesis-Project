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

    private int _detachedFrameCount;

    private void Start()
    {
        currentState = idleState;
        currentState.EnterState();
    }

    private void FixedUpdate()
    {
        // State logic
        currentState.FixedUpdateState();
        CheckCrouchingSurface();
    }

    private void CheckCrouchingSurface()
    {
        if (!PlayerData.Instance.IsCrouching) return;
        if (!PlayerData.Instance.isGrounded) return;
        if (!PlayerData.Instance.groundInfo.collider.CompareTag("Object")) return;
        
        if (PlayerData.Instance.fixedJoint) return;
        PlayerData.Instance.rigidBody.mass = 0.1f;
        PlayerData.Instance.fixedJoint = PlayerData.Instance.gameObject.AddComponent<FixedJoint>();
        PlayerData.Instance.fixedJoint.connectedBody = PlayerData.Instance.groundInfo.rigidbody;
        PlayerData.Instance.fixedJoint.enableCollision = true;
    }
}