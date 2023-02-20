
using UnityEngine;

public class PlayerFloatState : PlayerBaseState
{
    private float _timeElapsed;
    
    public override void EnterState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.useGravity = false;
    }

    public override void FixedUpdateState(PlayerVerticalStateManager manager)
    {
        _timeElapsed += Time.deltaTime;
        
        if (_timeElapsed > manager.floatTime)
        {
            manager.rigidBody.useGravity = true;
            _timeElapsed = 0f;
            manager.SwitchState(manager.fallState);
        }
    }
}
