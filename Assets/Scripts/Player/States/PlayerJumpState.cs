
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private int _frameElapsed;
    
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.playerData.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (_frameElapsed > 0)
        {
            if (controller.playerData.isGrounded)
            {
                ResetState(controller);
                controller.SwitchState(controller.groundedState);
                return;
            }
            
            if (controller.playerData.rigidBody.velocity.y < 0)
            {
                ResetState(controller);
                controller.SwitchState(controller.floatState);
                return;
            }
        }

        _frameElapsed++;
    }

    private void ResetState(VerticalStateController controller)
    {
        _frameElapsed = 0;
    }
}