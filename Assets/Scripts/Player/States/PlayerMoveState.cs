using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void FixedUpdateState(HorizontalStateController controller)
    {
        // Input and movement handling
        MoveAndRotateMesh(controller);
        LimitSpeed(controller);
            
        // State change
        if (controller.playerData.moveDirection == Vector2.zero) controller.SwitchState(controller.idleState);
    }

    private void RotateMesh(HorizontalStateController controller, Vector3 direction)
    {
        controller.playerData.meshes.forward = Vector3.Slerp(controller.playerData.meshes.forward, direction.normalized, Time.deltaTime * controller.playerData.movementRotationSpeed);
    }
    
    private void MoveMesh(HorizontalStateController controller, Vector3 direction)
    {
        controller.playerData.rigidBody.AddForce(direction.normalized * controller.playerData.acceleration, ForceMode.Acceleration);
    }

    private void MoveAndRotateMesh(HorizontalStateController controller)
    {
        Vector3 moveDirection, lookDirection;
        moveDirection = lookDirection = controller.playerData.orientation.forward * controller.playerData.moveDirection.y + controller.playerData.orientation.right * controller.playerData.moveDirection.x;

        if (controller.playerData.isOnSlope)
            moveDirection = Vector3.ProjectOnPlane(moveDirection, controller.playerData.groundInfo.normal).normalized;
        MoveMesh(controller, moveDirection);

        if (controller.playerData.isAiming) lookDirection = controller.playerData.orientation.forward;
        RotateMesh(controller, lookDirection);
    }

    private void LimitSpeed(HorizontalStateController controller)
    {
        if (controller.playerData.isOnSlope &&
            controller.playerData.rigidBody.velocity.magnitude > controller.playerData.maxSpeed)
        {
            controller.playerData.rigidBody.velocity = controller.playerData.rigidBody.velocity.normalized *
                                                       controller.playerData.maxSpeed;
            return;
        }

        Vector3 currentVelocity = controller.playerData.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
    
        if (xzVelocity.magnitude > controller.playerData.maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * controller.playerData.maxSpeed;
            controller.playerData.rigidBody.velocity = new Vector3(currentVelocity.x, controller.playerData.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}