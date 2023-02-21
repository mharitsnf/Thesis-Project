using System;
using UnityEngine;

public class VerticalStateController : BaseStateController
{
    // States
    public readonly PlayerGroundedState groundedState = new();
    public readonly PlayerFallState fallState = new();
    public readonly PlayerFloatState floatState = new();
    public readonly PlayerJumpState jumpState = new();

    private new void Awake()
    {
        base.Awake();
        playerData.playerYCenter = GetComponentInChildren<CapsuleCollider>().height * 0.5f;
    }

    private void Start()
    {
        GroundAndSlopeCheck();
        
        if (playerData.isGrounded) currentState = groundedState;
        else currentState = fallState;
        
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        GroundAndSlopeCheck();
        AddForceOnSlope();
        currentState.FixedUpdateState(this);
    }

    private void GroundAndSlopeCheck()
    {
        playerData.isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.25f, out playerData.groundInfo, playerData.playerYCenter + .05f);
        
        if (!playerData.isGrounded)
        {
            playerData.isOnSlope = false;
            playerData.rigidBody.useGravity = true;
            return;
        }

        float groundAngle = Vector3.Angle(Vector3.up, playerData.groundInfo.normal);
        playerData.isOnSlope = groundAngle < playerData.maxSlopeAngle && groundAngle != 0;

        if (playerData.isOnSlope) playerData.rigidBody.useGravity = false;
    }

    private void AddForceOnSlope()
    {
    }

    public void Jump()
    {
        playerData.rigidBody.AddForce(Vector3.up * playerData.initialJumpForce, ForceMode.Impulse);
    }

    public override void SwitchState(PlayerBaseState state)
    {
        base.SwitchState(state);
        currentState.EnterState(this);
    }
}
