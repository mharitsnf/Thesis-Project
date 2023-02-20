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
        controller.playerData.meshes.forward = Vector3.Slerp(controller.playerData.meshes.forward, direction.normalized, Time.deltaTime * controller.rotationSpeed);
    }
    
    private void MoveMesh(HorizontalStateController controller, Vector3 direction)
    {
        controller.playerData.rigidBody.AddForce(direction.normalized * controller.acceleration, ForceMode.Acceleration);
    }

    private void MoveAndRotateMesh(HorizontalStateController controller)
    {
        Vector3 direction = controller.playerData.orientation.forward * controller.playerData.moveDirection.y + 
                            controller.playerData.orientation.right * controller.playerData.moveDirection.x;

        MoveMesh(controller, direction);

        if (controller.playerData.isAiming) direction = controller.playerData.orientation.forward;
        RotateMesh(controller, direction);
    }

    private void LimitSpeed(HorizontalStateController controller)
    {
        Vector3 currentVelocity = controller.playerData.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
    
        if (xzVelocity.magnitude > controller.maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * controller.maxSpeed;
            controller.playerData.rigidBody.velocity = new Vector3(currentVelocity.x, controller.playerData.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}