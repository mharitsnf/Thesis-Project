using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerStateManager : MonoBehaviour
{
    
    [Header("Player Data")]
    public Transform meshes;
    public Transform orientation;
    [HideInInspector] public Rigidbody rigidBody; // public because I want to edit these in separate states
    [HideInInspector] public CinemachineFollowController cinemachineFollowController;
    [HideInInspector] public InputHandler inputHandler;
    private float _playerYCenter;

    [Header("Movement And Rotation")] 
    public float rotationSpeed = 10; 
    public float acceleration = 100;
    public float maxSpeed = 10;
    public float groundDrag = 4;
    public float airDrag = 1f;
    
    [Header("Jumping")]
    public float maxJumpForce = 100;

    // Directions    
    [HideInInspector] public Vector3 lookDirection;
        
    // Condition variables
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool isGrounded;
    private RaycastHit _groundInfo;

    // State
    private PlayerBaseState _currentState;
    public PlayerBaseState previousState;
    public readonly PlayerIdleState idleState = new();
    public readonly PlayerMoveState moveState = new();
    public readonly PlayerJumpState jumpState = new();

    private void Awake()
    {
        SetupPlayerData();
    }

    private void Start()
    {
        _currentState = idleState;
        _currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        // Condition checks
        GroundCheck();
        FallCheck();

        // State logic
        _currentState.FixedUpdateState(this);
    }

    private void SetupPlayerData()
    {
        // Get player Height
        _playerYCenter = meshes.GetChild(0).GetComponent<CapsuleCollider>().height * .5f;

        // Get rigidbody and freeze its rotation
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        rigidBody.drag = groundDrag;

        // Get camera controller
        cinemachineFollowController = GetComponent<CinemachineFollowController>();

        // Get input handler
        inputHandler = GetComponent<InputHandler>();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.5f, out _groundInfo, _playerYCenter + .05f);
        if (isGrounded) rigidBody.drag = groundDrag;
        else rigidBody.drag = airDrag;
    }

    private void FallCheck()
    {
        isFalling = rigidBody.velocity.y < 0;
    }

    public void SwitchState(PlayerBaseState state)
    {
        previousState = _currentState;
        _currentState = state;
        state.EnterState(this);
    }
}