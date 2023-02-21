public class PlayerGroundedState : PlayerBaseState
{
    private int _frameElapsed;

    public override void EnterState(VerticalStateController controller)
    {
        controller.playerData.rigidBody.drag = controller.playerData.groundDrag;
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (!controller.playerData.isGrounded)
            controller.SwitchState(controller.fallState);
    }
}