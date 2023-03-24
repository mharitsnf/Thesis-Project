using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void FixedUpdateState()
    {
        // Input and movement handling
        Move();
        LimitXZSpeed();
        
        RotateMesh();
            
        PlayerData.Instance.animator.SetFloat(MoveSpeed, PlayerData.Instance.rigidBody.velocity.magnitude);

        // State change
        if (PlayerData.Instance.moveDirection == Vector2.zero) PlayerData.Instance.horizontalStateController.SwitchState(PlayerData.Instance.horizontalStateController.idleState);
    }
    
    private void CrouchMove()
    {
        if (!PlayerData.Instance.fixedJoint && !PlayerData.Instance.IsCrouching) return;

        PlayerData.Instance.rigidBody.mass = 0.1f;
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
        float dot = Vector3.Dot(direction, PlayerData.Instance.groundInfo.normal);
        
        if (dot < -.75f) return;

        float acceleration;
        if (PlayerData.Instance.isGrounded)
        {
            acceleration = PlayerData.Instance.IsCrouching ? PlayerData.Instance.crouchingAcceleration : PlayerData.Instance.acceleration;
        }
        else
        {
            acceleration = PlayerData.Instance.airAcceleration;
        }
        

        float percentage = 1 - Mathf.Abs(dot);
        percentage = (Mathf.Pow(percentage, PlayerData.Instance.slopeMoveExponent) -
                      Mathf.Pow(0, PlayerData.Instance.slopeMoveExponent)) /
                     (Mathf.Pow(1, PlayerData.Instance.slopeMoveExponent) -
                      Mathf.Pow(0, PlayerData.Instance.slopeMoveExponent));

        acceleration = PlayerData.Instance.isOnSlope ? acceleration * percentage : acceleration;
        if (PlayerData.Instance.isOnSlope) direction = Vector3.ProjectOnPlane(direction, PlayerData.Instance.groundInfo.normal).normalized;


        PlayerData.Instance.rigidBody.AddForce(direction.normalized * acceleration, ForceMode.Force);
    }

    private void LimitXZSpeed()
    {
        var vState = PlayerData.Instance.verticalStateController;
        float maxSpeed = !PlayerData.Instance.IsCrouching || vState.currentState != vState.groundedState
            ? PlayerData.Instance.maxSpeed
            : PlayerData.Instance.maxCrouchSpeed;
        
        Vector3 currentVelocity = PlayerData.Instance.rigidBody.velocity;
        Vector3 xzVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        if (xzVelocity.magnitude > maxSpeed)
        {
            currentVelocity = xzVelocity.normalized * maxSpeed;
            PlayerData.Instance.rigidBody.velocity = new Vector3(currentVelocity.x, PlayerData.Instance.rigidBody.velocity.y, currentVelocity.z);
        }
    }
}
