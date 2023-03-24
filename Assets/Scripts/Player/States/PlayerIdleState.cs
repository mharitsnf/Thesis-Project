using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    
    public override void FixedUpdateState()
    {
        RotateMesh();

        PlayerData.Instance.animator.SetFloat(MoveSpeed, Mathf.Lerp(PlayerData.Instance.animator.GetFloat(MoveSpeed), 0, PlayerData.Instance.toIdleWeight));

        // State change
        if (PlayerData.Instance.moveDirection != Vector2.zero) PlayerData.Instance.horizontalStateController.SwitchState(PlayerData.Instance.horizontalStateController.moveState);
    }

    private void CrouchIdle()
    {
        if (!PlayerData.Instance.fixedJoint && !PlayerData.Instance.IsCrouching) return;

        PlayerData.Instance.rigidBody.mass = PlayerData.Instance.mass;
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