
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private static readonly int JustLanded = Animator.StringToHash("JustLanded");
    
    public override void EnterState()
    {
        if (!PlayerData.Instance.isGrounded)
        {
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
            return;
        }
        
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.groundDrag;
        InteractionController.Instance.playerInput.CharacterControls.Jump.Enable();
        PlayerData.Instance.animator.SetBool(JustLanded, true);
    }

    public override void FixedUpdateState()
    {
        if (!PlayerData.Instance.isGrounded)
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
        
        SetResetPosition();
    }

    private void SetResetPosition()
    {
        if (PlayerData.Instance.groundInfo.Equals(default(RaycastHit))) return;
        if (PlayerData.Instance.groundInfo.collider.CompareTag("Object")) return;
        
        Vector3 offset = PlayerData.Instance.rigidBody.velocity.normalized;

        if (Physics.Raycast(PlayerData.Instance.transform.position + offset, Vector3.down, 10f))
        {
            PlayerData.Instance.resetPosition = PlayerData.Instance.transform.position - offset;
        }
    }

    public override void ExitState()
    {
        PlayerData.Instance.animator.SetBool(JustLanded, false);
    }
}