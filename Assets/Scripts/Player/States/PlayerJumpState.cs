
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private int _frameElapsed;
    
    public override void EnterState(VerticalStateController controller)
    {
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (_frameElapsed > 0)
        {
            if (PlayerData.Instance.isGrounded)
            {
                ExitState(controller);
                controller.SwitchState(controller.groundedState);
                return;
            }
            
            if (PlayerData.Instance.rigidBody.velocity.y < 0)
            {
                ExitState(controller);
                controller.SwitchState(controller.floatState);
                return;
            }
        }

        _frameElapsed++;
    }

    public override void ExitState(VerticalStateController controller)
    {
        _frameElapsed = 0;
    }
}