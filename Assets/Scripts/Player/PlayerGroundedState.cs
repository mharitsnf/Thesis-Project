
public class PlayerGroundedState : PlayerBaseState
{
    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.groundDrag;
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (!controller.playerData.isGrounded)
        { 
            if (controller.playerData.rigidBody.velocity.y > 0) controller.SwitchState(controller.jumpState);
            else controller.SwitchState(controller.fallState);
        }
    }
}
