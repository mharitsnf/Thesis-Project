
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.playerData.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (controller.playerData.isGrounded) controller.SwitchState(controller.groundedState);
    }
}
