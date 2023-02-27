
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
    }
    
    public override void FixedUpdateState()
    {
        PlayerData.Instance.rigidBody.AddForce(Physics.gravity * PlayerData.Instance.fallGravityMultiplier, ForceMode.Acceleration);
        
        if (PlayerData.Instance.isGrounded)
            PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
    }

    public override void ExitState()
    {
        PlayerData.Instance.wasJumping = false;
    }
}
