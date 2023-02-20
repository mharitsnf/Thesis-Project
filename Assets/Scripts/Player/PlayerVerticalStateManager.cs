using System;
using UnityEngine;

public class PlayerVerticalStateManager : PlayerBaseManager
{
 
    private float _playerYCenter;

    private RaycastHit _groundInfo;
    public bool isJumping;
    private bool isGrounded;
    private bool isFalling;

    // States
    public readonly PlayerGroundedState groundedState = new();
    public readonly PlayerJumpState jumpState = new();

    private void FixedUpdate()
    {
        GroundCheck();
        FallCheck();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.5f, out _groundInfo, _playerYCenter + .05f);
    }
    
    private void FallCheck()
    {
        isFalling = rigidBody.velocity.y < 0;
    }

    public override void SwitchState(PlayerBaseState state)
    {
        base.SwitchState(state);
        currentState.EnterState(this);
    }
}
