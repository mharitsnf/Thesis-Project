
using UnityEngine;

public class PlayerFloatState : PlayerBaseState
{
    private float _timeElapsed;
    
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.useGravity = false;
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        _timeElapsed += Time.deltaTime;
        
        if (_timeElapsed > controller.playerData.floatTime)
        {
            controller.playerData.rigidBody.useGravity = true;
            _timeElapsed = 0f;
            controller.SwitchState(controller.fallState);
        }
    }
}
