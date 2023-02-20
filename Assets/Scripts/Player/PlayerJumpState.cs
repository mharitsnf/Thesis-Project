﻿
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.drag = manager.airDrag;
    }
    
    public override void FixedUpdateState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.AddForce(Physics.gravity * manager.jumpGravityMultiplier);

        if (manager.rigidBody.velocity.y < 0) manager.SwitchState(manager.floatState);
    }
}