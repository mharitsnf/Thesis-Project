using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void FixedUpdateState(HorizontalStateController controller)
    {
        RotateMesh(controller);
        
        // State change
        if (controller.playerData.moveDirection != Vector2.zero) controller.SwitchState(controller.moveState);
    }

    private void RotateMesh(HorizontalStateController controller)
    {
        if (controller.playerData.isAiming)
        {
            Vector3 lookDirection = controller.playerData.orientation.forward;
            controller.playerData.meshes.forward = Vector3.Slerp(controller.playerData.meshes.forward, lookDirection.normalized, Time.deltaTime * controller.playerData.movementRotationSpeed);
        }
    }
}