
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.drag = manager.airDrag;
    }
    
    public override void FixedUpdateState(PlayerVerticalStateManager manager)
    {
        manager.rigidBody.AddForce(Physics.gravity * manager.fallGravityMultiplier);
        
        if (manager.isGrounded) manager.SwitchState(manager.groundedState);
    }
}
