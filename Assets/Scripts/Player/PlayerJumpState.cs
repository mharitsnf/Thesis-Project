
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.AddForce(Physics.gravity * controller.jumpGravityMultiplier);

        if (controller.playerData.rigidBody.velocity.y < 0) controller.SwitchState(controller.floatState);
    }
}