using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager manager)
    {
    }

    public override void FixedUpdateState(PlayerStateManager manager)
    {
        RotateMesh(manager);
        
        // State change
        if (manager.inputHandler.moveDirection != Vector2.zero) manager.SwitchState(manager.moveState);
    }

    private void RotateMesh(PlayerStateManager manager)
    {
        if (manager.inputHandler.isAiming)
        {
            Vector3 lookDirection = manager.orientation.forward;
            manager.meshes.forward = Vector3.Slerp(manager.meshes.forward, lookDirection.normalized, Time.deltaTime * manager.rotationSpeed);
        }
    }
}