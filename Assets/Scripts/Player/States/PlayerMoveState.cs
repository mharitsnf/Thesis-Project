using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void FixedUpdateState(HorizontalStateController controller)
    {
        
        // Input and movement handling
        if (!PlayerData.Instance.joint ||
            PlayerData.Instance.verticalStateController.currentState == PlayerData.Instance.verticalStateController.groundedState)
            Move();
        else AirMove();
        
        RotateMesh();
        LimitSpeed();
            
        // State change
        if (PlayerData.Instance.moveDirection == Vector2.zero) controller.SwitchState(controller.idleState);
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
        
        if (PlayerData.Instance.isOnSlope) direction = Vector3.ProjectOnPlane(direction, PlayerData.Instance.groundInfo.normal).normalized;
        
        PlayerData.Instance.rigidBody.AddForce(direction.normalized * (PlayerData.Instance.acceleration), ForceMode.Force);
    }

    private void AirMove()
    {
        Vector3 direction = PlayerData.Instance.orientation.forward * Mathf.Max(PlayerData.Instance.moveDirection.y, 0) +
                            PlayerData.Instance.orientation.right * PlayerData.Instance.moveDirection.x;
        direction = direction.normalized;
        direction.x *= PlayerData.Instance.horizontalAirThrust;
        direction.z *= PlayerData.Instance.verticalAirThrust;

        PlayerData.Instance.rigidBody.AddForce(direction, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        if (PlayerData.Instance.joint && PlayerData.Instance.rigidBody.velocity.magnitude > PlayerData.Instance.maxSwingSpeed)
        {
            PlayerData.Instance.rigidBody.velocity = PlayerData.Instance.rigidBody.velocity.normalized *
                                                       PlayerData.Instance.maxSwingSpeed;
            return;
        }
        
        if (PlayerData.Instance.isOnSlope && PlayerData.Instance.rigidBody.velocity.magnitude > PlayerData.Instance.maxSpeed)
        {
            PlayerData.Instance.rigidBody.velocity = PlayerData.Instance.rigidBody.velocity.normalized *
                                                       PlayerData.Instance.maxSpeed;
            return;
        }

        Vector3 currentVelocity = PlayerData.Instance.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
    
        if (xzVelocity.magnitude > PlayerData.Instance.maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * PlayerData.Instance.maxSpeed;
            PlayerData.Instance.rigidBody.velocity = new Vector3(currentVelocity.x, PlayerData.Instance.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}