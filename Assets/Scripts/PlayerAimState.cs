using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager manager)
    {
    }

    public override void FixedUpdateState(PlayerStateManager manager)
    {
        // Input and movement handling
        //RotateMesh(manager);
        //UpdateMoveDirection(manager);
        //Move(manager);
        //LimitSpeed(manager);
        
        //if (!manager.isAiming) manager.SwitchState(manager.previousState);
    }
    
    // private void RotateMesh(PlayerStateManager manager)
    // {
    //     manager.meshes.forward = Vector3.Slerp(manager.meshes.forward, manager.orientation.forward, Time.deltaTime * manager.rotationSpeed);
    // }
    //
    // private void UpdateMoveDirection(PlayerStateManager manager)
    // {
    //     //Vector2 inputDirection = manager.PlayerInput.CharacterControls.Move.ReadValue<Vector2>();
    //     //Transform orientation = manager.orientation;
    //
    //     //manager.moveDirection = orientation.forward * inputDirection.y + orientation.right * inputDirection.x;
    // }
    //     
    // private void Move(PlayerStateManager manager)
    // {
    //     //int isMoving = (manager.moveDirection == Vector3.zero) ? 0 : 1;
    //     //manager.rigidBody.AddForce(manager.orientation.forward.normalized * (isMoving * manager.acceleration), ForceMode.Acceleration);
    //     manager.rigidBody.AddForce(manager.moveDirection.normalized * manager.acceleration, ForceMode.Acceleration);
    // }
    //     
    // private void LimitSpeed(PlayerStateManager manager)
    // {
    //     Vector3 currentVelocity = manager.rigidBody.velocity;
    //     Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
    //
    //     if (xzVelocity.magnitude > manager.maxSpeed)
    //     {
    //         currentVelocity = xzVelocity.normalized * manager.maxSpeed;
    //         manager.rigidBody.velocity = new Vector3(currentVelocity.x, manager.rigidBody.velocity.y, currentVelocity.z);
    //     }
    // }
}
