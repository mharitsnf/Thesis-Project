
using UnityEngine;

public class PlayerFloatState : PlayerBaseState
{
    private int _frameElapsed;
    
    // public override void EnterState()
    // {
    //     if (PlayerData.Instance.isGrounded)
    //     {
    //         PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
    //     }
    // }
    //
    // public override void FixedUpdateState()
    // {
    //     if (PlayerData.Instance.isGrounded)
    //     {
    //         PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.groundedState);
    //         return;
    //     }
    //
    //     if (_frameElapsed > PlayerData.Instance.floatFrameLimit)
    //     {
    //         PlayerData.Instance.verticalStateController.SwitchState(PlayerData.Instance.verticalStateController.fallState);
    //     }
    //
    //     _frameElapsed++;
    // }
    //
    // public override void ExitState()
    // {
    //     _frameElapsed = 0;
    // }
}
