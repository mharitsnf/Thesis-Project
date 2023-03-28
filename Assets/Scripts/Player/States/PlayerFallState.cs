
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float _timeSinceEntered;
    
    public override void EnterState()
    {
        if (PlayerData.Instance.isGrounded)
        {
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
            return;
        }
        
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.airDrag;
        
        PlayerData.Instance.animator.SetBool("JustFell", true);
    }
    
    public override void FixedUpdateState()
    {
        if (_timeSinceEntered > PlayerData.Instance.coyoteTime && InteractionController.Instance.playerInput.CharacterControls.Jump.enabled)
        {
            InteractionController.Instance.playerInput.CharacterControls.Jump.Disable();
        }

        _timeSinceEntered += Time.fixedDeltaTime;
        
        if (PlayerData.Instance.isGrounded)
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
    }

    public override void ExitState()
    {
        PlayerData.Instance.wasJumping = false;
        
        PlayerData.Instance.animator.SetBool("JustFell", false);

        _timeSinceEntered = 0;
    }
}
