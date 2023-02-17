using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager manager)
    {
    }

    public override void FixedUpdateState(PlayerStateManager manager)
    {
        // Input and movement handling
        MoveAndRotateMesh(manager);
        LimitSpeed(manager);
            
        // State change
        if (manager.inputHandler.moveDirection == Vector2.zero) manager.SwitchState(manager.idleState);
    }
        
    private void RotateMesh(PlayerStateManager manager, Vector3 direction)
    {
        manager.meshes.forward = Vector3.Slerp(manager.meshes.forward, direction.normalized, Time.deltaTime * manager.rotationSpeed);
    }
    
    private void MoveMesh(PlayerStateManager manager, Vector3 direction)
    {
        manager.rigidBody.AddForce(direction.normalized * manager.acceleration, ForceMode.Acceleration);
    }

    private void MoveAndRotateMesh(PlayerStateManager manager)
    {
        Vector3 direction = manager.orientation.forward * manager.inputHandler.moveDirection.y + 
                            manager.orientation.right * manager.inputHandler.moveDirection.x;

        MoveMesh(manager, direction);

        if (manager.inputHandler.isAiming) direction = manager.orientation.forward;
        RotateMesh(manager, direction);
    }

    private void LimitSpeed(PlayerStateManager manager)
    {
        Vector3 currentVelocity = manager.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
    
        if (xzVelocity.magnitude > manager.maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * manager.maxSpeed;
            manager.rigidBody.velocity = new Vector3(currentVelocity.x, manager.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}