
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.AddForce(Physics.gravity * controller.fallGravityMultiplier);
        
        if (controller.playerData.isGrounded) controller.SwitchState(controller.groundedState);
    }
}
