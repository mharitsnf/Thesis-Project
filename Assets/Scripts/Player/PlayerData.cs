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
    [ReadOnly] public GameObject crosshairObject;
    
    [Header(("Controllers"))]
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;
    [ReadOnly] public CameraController cameraController;

    [Header("Movement Forces")]
    public float acceleration = 100;
    public float airAcceleration = 100;
    public float maxJumpForce;
    
    [Header("Horizontal Movement Limitations")] 
    public float movementRotationSpeed = 10; 
    public float maxSpeed = 10;
    public float maxSwingSpeed = 20;

    [Header("Ground Check Data")]
    public float mass = 50;
    public float maxSlopeAngle;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isOnSlope;
    [ReadOnly] public bool isOnExtremeSlope;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;
    
    [Header("Jumping")]
    [HideInInspector] public float currentJumpPercentage = 1;
    public float buttonHoldTime;
    public bool wasJumping;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Camera")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    public float cameraRotationSpeed;
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

    [Header("Spring Data")]
    public float ropeRayCastDistance;
    public float maxDistanceMultiplier;
    public float minDistanceMultiplier;
    public float springinessMultiplier;
    public float springDamper;
    public float springMassScale;

    public enum InteractionState {RopePlacement, Attaching}

    [Header("Interaction")]
    public InteractionState currentInteractionState;
    
    [Header("Rope Data")]
    public GameObject ropePrefab;
    public readonly LinkedList<LinkedList<GameObject>> placedRopes = new();
    public readonly LinkedList<GameObject> activeRopes = new();
    public bool isAiming;
    public RaycastHit selectedGameObject = new();

    [Header("Joint Data")]
    public float attachRaycastDistance;
    public FixedJoint fixedJoint;

    [Header("Time")]
    public float aimingTimeScale = 0.25f;

    [Header("Objective Collected")]
    public int objectiveCollectedAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        currentInteractionState = InteractionState.RopePlacement;
    }

    private void Start()
    {
        initialPosition = transform.position;
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
