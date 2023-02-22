
public class PlayerGroundedState : PlayerBaseState
{
    private int _frameElapsed;

    public override void EnterState(VerticalStateController controller)
    {
        if (!PlayerData.Instance.isGrounded)
        {
            controller.SwitchState(controller.fallState);
            return;
        }
        
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.groundDrag;
    }

    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (!PlayerData.Instance.isGrounded)
            controller.SwitchState(controller.fallState);
    }
}