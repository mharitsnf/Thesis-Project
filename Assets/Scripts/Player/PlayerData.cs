using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// Holds instances to the object's components and data
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    [Header("Player Objects and Components")]
    [ReadOnly] public Transform meshes;
    [ReadOnly] public Transform orientation;
    [ReadOnly] public GameObject cinemachineFollow;
    [ReadOnly] public Rigidbody rigidBody;
    [ReadOnly] public Vector3 initialPosition;
    [ReadOnly] public Quaternion initialRotation;
    [ReadOnly] public GameObject crosshairObject;
    [ReadOnly] public Animator animator;
    
    [Header(("Controllers"))]
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;
    [ReadOnly] public CameraController cameraController;
    [ReadOnly] public RopePlacementController ropePlacementController;

    [Header("Animation")]
    [Range(0f, 1f)]
    public float feetDistanceToGround;
    [Range(0f, 1f)]
    public float toIdleWeight;
    
    [Header("Movement Forces")]
    public float acceleration = 100;
    public float crouchingAcceleration = 400;
    public float airAcceleration = 100;
    public float maxJumpForce;
    public float slopeMoveExponent = 0.3f;
    
    [Header("Horizontal Movement Limitations")] 
    public float movementRotationSpeed = 10; 
    public float maxSpeed = 10;

    [Header("Ground Check Data")]
    public float mass = 50;
    public float maxSlopeAngle;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isOnSlope;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;
    
    [Header("Jumping")]
    public float currentJumpPercentage = 1;
    public float jumpExponent;
    public float buttonHoldTime;
    public float coyoteTime;
    public bool wasJumping;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Camera")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    [Range(0,1)]
    public float cameraLerpWeight;
    public float cameraBrakeWeight;
    public float thirdPersonCameraSpeed;
    public float aimCameraSpeed;
    public float minClamp;
    public float maxClamp;
    public bool invertY;
    public bool invertX;
    
    [Header("External Objects")]
    [ReadOnly] public Transform realCamera;

    [Header("Input Data")]
    [ReadOnly] public Vector2 moveDirection;
    [ReadOnly] public Vector2 cameraLookDelta;
    [ReadOnly] public float cinemachineFollowYaw;
    [ReadOnly] public float cinemachineFollowPitch;

    [Header("Crouching")]
    public float maxCrouchSpeed = 25f;
    public FixedJoint fixedJoint;
    private bool _isCrouching;
    public bool IsCrouching
    {
        get => _isCrouching;
        set
        {
            _isCrouching = value;
            animator.SetBool(Crouching, value);
            if (value || !fixedJoint) return;
            rigidBody.mass = mass;
            Destroy(fixedJoint);
            fixedJoint = null;
        }
    }

    [Header("Spring Data")]
    public float ropeRayCastDistance;
    public float maxDistanceMultiplier;
    public float minDistanceMultiplier;
    public float springinessMultiplier;
    public float springDamper;
    public float springMassScale;

    public enum InteractionState {RopePlacement}

    [Header("Interaction")]
    public InteractionState currentInteractionState;

    [Header("Pushing")] 
    public float pushForce;
    public float pushingAnimationSmoothness;

    [Header("Rope Data")]
    public LayerMask selectableSurfaces;
    public GameObject ropePrefab;
    public readonly LinkedList<LinkedList<GameObject>> placedRopes = new();
    public readonly LinkedList<GameObject> activeRopes = new();
    public bool isAiming;
    public RaycastHit selectedGameObject = new();

    [Header("Time")]
    public float aimingTimeScale = 0.25f;
    public float resetPositionTime;

    [Header("Objective Collected")]
    public int objectiveCollectedAmount;

    private static readonly int Crouching = Animator.StringToHash("IsCrouching");

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        currentInteractionState = InteractionState.RopePlacement;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rigidBody.mass = mass;
    }

    public GameObject TryPeekActiveRope()
    {
        try
        {
            return activeRopes.Last.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
