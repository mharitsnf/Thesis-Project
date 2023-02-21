
using UnityEngine;

public class PlayerFloatState : PlayerBaseState
{
    private float _timeElapsed;
    
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.useGravity = false;
        
        if (controller.playerData.isGrounded)
        {
            ResetState(controller);
            controller.SwitchState(controller.groundedState);
        }
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        _timeElapsed += Time.deltaTime;
        
        if (controller.playerData.isGrounded)
        {
            ResetState(controller);
            controller.SwitchState(controller.groundedState);
            return;
        }

        if (_timeElapsed > controller.playerData.floatTime)
        {
            ResetState(controller);
            controller.SwitchState(controller.fallState);
        }
    }

    private void ResetState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.useGravity = true;
        _timeElapsed = 0f;
    }
}
