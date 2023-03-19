using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void FixedUpdateState()
    {
        // Input and movement handling
        Move();
        
        RotateMesh();
        LimitSpeed();
            
        PlayerData.Instance.animator.SetFloat(MoveSpeed, PlayerData.Instance.rigidBody.velocity.magnitude);

        // State change
        if (PlayerData.Instance.moveDirection == Vector2.zero) PlayerData.Instance.horizontalStateController.SwitchState(PlayerData.Instance.horizontalStateController.idleState);
    }

    private void RotateMesh()
    {
        Vector3 direction = PlayerData.Instance.orientation.forward * PlayerData.Instance.moveDirection.y +
                            PlayerData.Instance.orientation.right * PlayerData.Instance.moveDirection.x;
        
        PlayerData.Instance.meshes.forward = Vector3.Slerp(PlayerData.Instance.meshes.forward, direction.normalized, Time.deltaTime * PlayerData.Instance.movementRotationSpeed);
    }
    
    private void Move()
    {
        Vector3 direction = PlayerData.Instance.orientation.forward * PlayerData.Instance.moveDirection.y +
                            PlayerData.Instance.orientation.right * PlayerData.Instance.moveDirection.x;
        
        float acceleration = PlayerData.Instance.isGrounded
            ? PlayerData.Instance.acceleration
            : PlayerData.Instance.airAcceleration;
        
        if (PlayerData.Instance.isOnSlope) direction = Vector3.ProjectOnPlane(direction, PlayerData.Instance.groundInfo.normal).normalized;
        
        PlayerData.Instance.rigidBody.AddForce(direction.normalized * acceleration, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        if (PlayerData.Instance.isOnSlope && PlayerData.Instance.rigidBody.velocity.magnitude > PlayerData.Instance.maxSpeed)
        {
            PlayerData.Instance.rigidBody.velocity = PlayerData.Instance.rigidBody.velocity.normalized *
                                                       PlayerData.Instance.maxSpeed;
            return;
        }

        Vector3 currentVelocity = PlayerData.Instance.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        var vState = PlayerData.Instance.verticalStateController;
        
        float maxSpeed = !PlayerData.Instance.IsCrouching || vState.currentState != vState.groundedState
            ? PlayerData.Instance.maxSpeed
            : PlayerData.Instance.maxCrouchSpeed;
    
        if (xzVelocity.magnitude > maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * maxSpeed;
            PlayerData.Instance.rigidBody.velocity = new Vector3(currentVelocity.x, PlayerData.Instance.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}