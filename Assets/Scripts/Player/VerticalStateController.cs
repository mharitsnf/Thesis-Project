using System;
using UnityEngine;

public class VerticalStateController : BaseStateController
{
    // States
    public readonly PlayerGroundedState groundedState = new();
    public readonly PlayerFallState fallState = new();
    public readonly PlayerJumpState jumpState = new();

    private int _standingTime;

    private void Start()
    {
        PlayerData.Instance.playerYCenter = GetComponentInChildren<CapsuleCollider>().height * 0.5f;
        GroundAndSlopeCheck();
        
        if (PlayerData.Instance.isGrounded) currentState = groundedState;
        else currentState = fallState;
        
        currentState.EnterState();
    }

    private void FixedUpdate()
    {
        ResetPosition();
        GroundAndSlopeCheck();
        currentState.FixedUpdateState();
    }

    private void GroundAndSlopeCheck()
    {
        float multiplier = 1f;
        PlayerData.Instance.isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down * multiplier), 0.25f, out PlayerData.Instance.groundInfo, PlayerData.Instance.playerYCenter + .05f);

        if (!PlayerData.Instance.isGrounded)
        {
            PlayerData.Instance.isOnSlope = false;
            return;
        }

        float groundAngle = Vector3.Angle(Vector3.up, PlayerData.Instance.groundInfo.normal);
        PlayerData.Instance.isOnSlope = groundAngle < PlayerData.Instance.maxSlopeAngle && groundAngle != 0;

        if (PlayerData.Instance.groundInfo.collider.CompareTag("Object")) return;
        if (_standingTime < 4)
        {
            _standingTime++;
            return;
        }
        
        Vector3 velocity = PlayerData.Instance.rigidBody.velocity.normalized;
        Vector3 offset = new Vector3(velocity.x, 0f, velocity.z);
        PlayerData.Instance.initialPosition = PlayerData.Instance.transform.position;
        _standingTime = 0;
    }

    private void ResetPosition()
    {
        if (!(transform.position.y < -20f)) return;
        PlayerData.Instance.IsCrouching = false;
        PlayerData.Instance.rigidBody.velocity = Vector3.zero;
        PlayerData.Instance.rigidBody.angularVelocity = Vector3.zero;
        PlayerData.Instance.transform.position = PlayerData.Instance.initialPosition;
    }

    public void Jump()
    {
        SwitchState(jumpState);
    }
}
