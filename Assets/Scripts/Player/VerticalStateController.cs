using System;
using UnityEngine;

public class VerticalStateController : BaseStateController
{
    // States
    public readonly PlayerGroundedState groundedState = new();
    public readonly PlayerFallState fallState = new();
    public readonly PlayerJumpState jumpState = new();

    private float _standingTime;

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
    }

    private void ResetPosition()
    {
        if (!(transform.position.y < 1f)) return;
        Vector3 resetPosition = PlayerData.Instance.resetPosition;

        if (!Physics.Raycast(resetPosition, Vector3.down, 5))
        {
            resetPosition = PlayerData.Instance.firstPosition;
        }

        PlayerData.Instance.IsCrouching = false;
        PlayerData.Instance.rigidBody.velocity = Vector3.zero;
        PlayerData.Instance.rigidBody.angularVelocity = Vector3.zero;
        PlayerData.Instance.transform.position = resetPosition;
    }

    public void Jump()
    {
        SwitchState(jumpState);
    }
}
