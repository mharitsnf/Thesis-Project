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
    
    [Header(("Controllers"))]
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;

    [Header("Movement Forces")]
    public bool isAffectedByMass;
    public float acceleration = 100;
    public float airAcceleration = 100;
    public float maxJumpForce;
    
    [Header("Horizontal Movement Limitations")] 
    public float movementRotationSpeed = 10; 
    public float maxSpeed = 10;
    public float maxSwingSpeed = 20;

    [Header("Ground Check Data")]
    public float maxSlopeAngle;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isOnSlope;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;
    
    [Header("Jumping")]
    [HideInInspector] public float currentJumpPercentage = 1;
    public float buttonHoldTime;
    public bool wasJumping;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Gravity")]
    public float fallGravityMultiplier;
    public float jumpGravityMultiplier;
    
    [Header("Camera")]
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
    public float rayCastDistance;
    public float maxRopeDistance;
    public float minRopeDistance;
    public float springSpringiness;
    public float springDamper;
    public float springMassScale;

    [Header("Rope Data")]
    public GameObject ropePrefab;
    public readonly Stack<Stack<GameObject>> placedRopes = new();
    public readonly Stack<GameObject> activeRopes = new();
    public bool isSelecting;
    public RaycastHit selectedGameObject = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        if (isAffectedByMass)
        {
            float mass = rigidBody.mass;
            acceleration *= mass;
            airAcceleration *= mass;
            maxJumpForce *= mass;
        }
    }

    public GameObject TryPeekActiveRope()
    {
        try
        {
            return activeRopes.Peek();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
