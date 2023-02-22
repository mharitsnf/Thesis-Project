
using UnityEngine;

public class PlayerFloatState : PlayerBaseState
{
    private float _timeElapsed;
    
    public override void EnterState(VerticalStateController controller)
    {
        if (PlayerData.Instance.isGrounded)
        {
            ExitState(controller);
            controller.SwitchState(controller.groundedState);
            return;
        }
        
        PlayerData.Instance.rigidBody.useGravity = false;
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        _timeElapsed += Time.deltaTime;
        
        if (PlayerData.Instance.isGrounded)
        {
            ExitState(controller);
            controller.SwitchState(controller.groundedState);
            return;
        }

        if (_timeElapsed > PlayerData.Instance.floatTime)
        {
            ExitState(controller);
            controller.SwitchState(controller.fallState);
        }
    }

    public override void ExitState(VerticalStateController controller)
    {
        PlayerData.Instance.rigidBody.useGravity = true;
        _timeElapsed = 0f;
    }
}
