
public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(VerticalStateController controller)
    {
        if (PlayerData.Instance.isGrounded)
        {
            controller.SwitchState(controller.groundedState);
            return;
        }
        
        PlayerData.Instance.rigidBody.drag = PlayerData.Instance.airDrag;
    }
    
    public override void FixedUpdateState(VerticalStateController controller)
    {
        if (PlayerData.Instance.isGrounded)
            controller.SwitchState(controller.groundedState);
    }
}
