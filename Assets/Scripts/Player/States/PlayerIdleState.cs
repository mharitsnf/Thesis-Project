using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void FixedUpdateState(HorizontalStateController controller)
    {
        RotateMesh(controller);
        
        // State change
        if (PlayerData.Instance.moveDirection != Vector2.zero) controller.SwitchState(controller.moveState);
    }

    private void RotateMesh(HorizontalStateController controller)
    {
        if (PlayerData.Instance.isAiming)
        {
            Vector3 lookDirection = PlayerData.Instance.orientation.forward;
            PlayerData.Instance.meshes.forward = Vector3.Slerp(PlayerData.Instance.meshes.forward, lookDirection.normalized, Time.deltaTime * PlayerData.Instance.movementRotationSpeed);
        }
    }
}