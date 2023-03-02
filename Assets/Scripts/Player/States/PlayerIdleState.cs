using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void FixedUpdateState()
    {
        RotateMesh();
        
        // State change
        if (PlayerData.Instance.moveDirection != Vector2.zero) PlayerData.Instance.horizontalStateController.SwitchState(PlayerData.Instance.horizontalStateController.moveState);
    }

    private void RotateMesh()
    {
        if (PlayerData.Instance.isAiming)
        {
            Vector3 lookDirection = PlayerData.Instance.orientation.forward;
            PlayerData.Instance.meshes.forward = Vector3.Slerp(PlayerData.Instance.meshes.forward,
                lookDirection.normalized, Time.deltaTime * PlayerData.Instance.movementRotationSpeed);
        }
       
    }
}