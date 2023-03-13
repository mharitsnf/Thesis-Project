
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
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
        
        if (PlayerData.Instance.isGrounded)
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
    }

    public override void ExitState()
    {
        PlayerData.Instance.wasJumping = false;
        
        PlayerData.Instance.animator.SetBool("JustFell", false);
    }
}
