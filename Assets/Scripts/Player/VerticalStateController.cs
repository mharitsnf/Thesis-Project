using System;
using UnityEngine;

public class VerticalStateController : BaseStateController
{
    // States
    public readonly PlayerGroundedState groundedState = new();
    public readonly PlayerFallState fallState = new();
    public readonly PlayerJumpState jumpState = new();

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
        PlayerData.Instance.isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.25f, out PlayerData.Instance.groundInfo, PlayerData.Instance.playerYCenter + .05f);

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
        if (transform.position.y < -50f) transform.position = PlayerData.Instance.initialPosition;
    }

    public void Jump()
    {
        SwitchState(jumpState);
    }
}
