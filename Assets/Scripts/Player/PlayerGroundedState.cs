
public class PlayerGroundedState : PlayerBaseState
{
    public override void EnterState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.drag = manager.groundDrag;
    }

    public override void FixedUpdateState(PlayerVerticalStateManager manager)
    {
        if (!manager.isGrounded)
        { 
            if (manager.rigidBody.velocity.y > 0) manager.SwitchState(manager.jumpState);
            else manager.SwitchState(manager.fallState);
        }
    }
}
