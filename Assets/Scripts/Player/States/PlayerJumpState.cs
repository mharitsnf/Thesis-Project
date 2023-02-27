
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private int _frameElapsed;
    
    public override void EnterState()
    {
        PlayerData.Instance.wasJumping = true;
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.airDrag;
        PlayerData.Instance.rigidBody.AddForce(Vector3.up * (PlayerData.Instance.maxJumpForce * PlayerData.Instance.currentJumpPercentage), ForceMode.Impulse);
    }
    
    public override void FixedUpdateState()
    {
        PlayerData.Instance.rigidBody.AddForce(Physics.gravity * PlayerData.Instance.jumpGravityMultiplier,
            ForceMode.Acceleration);
        
        if (_frameElapsed > 1)
        {
            if (PlayerData.Instance.isGrounded)
            {
                PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
                return;
            }
            
            if (PlayerData.Instance.rigidBody.velocity.y < 2.5f)
            {
                PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
                return;
            }
        }

        _frameElapsed++;
    }

    public override void ExitState()
    {
        _frameElapsed = 0;
        PlayerData.Instance.currentJumpPercentage = 1;
    }
}