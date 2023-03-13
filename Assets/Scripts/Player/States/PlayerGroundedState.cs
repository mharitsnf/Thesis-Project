
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private int _frameElapsed;

    public override void EnterState()
    {
        if (!PlayerData.Instance.isGrounded)
        {
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
            return;
        }
        
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.groundDrag;
        
        PlayerData.Instance.animator.SetBool("JustLanded", true);
    }

    public override void FixedUpdateState()
    {
        
        if (!PlayerData.Instance.isGrounded)
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
    }

    public override void ExitState()
    {
        PlayerData.Instance.animator.SetBool("JustLanded", false);
    }
}