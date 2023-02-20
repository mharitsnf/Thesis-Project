using System;
using UnityEngine;

public class VerticalStateController : BaseStateController
{
    [Header("Jumping")]
    public float initialJumpForce;
    public float floatTime;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Gravity")]
    public float fallGravityMultiplier;
    public float jumpGravityMultiplier;

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
        GroundCheck();
        
        if (playerData.isGrounded) currentState = groundedState;
        else currentState = fallState;
        
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        
        currentState.FixedUpdateState(this);
    }

    private void GroundCheck()
    {
        playerData.isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.5f, out playerData.groundInfo, playerData.playerYCenter + .05f);
    }

    public void Jump()
    {
        playerData.rigidBody.AddForce(Vector3.up * initialJumpForce, ForceMode.Impulse);
    }

    public override void SwitchState(PlayerBaseState state)
    {
        base.SwitchState(state);
        currentState.EnterState(this);
    }
}
