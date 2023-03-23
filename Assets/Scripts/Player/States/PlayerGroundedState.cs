
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private int _frameElapsed;
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
    }

    public override void ExitState()
    {
        InteractionController.Instance.playerInput.CharacterControls.Jump.Disable();
        PlayerData.Instance.animator.SetBool(JustLanded, false);
    }
}