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
    [ReadOnly] public Transform grapplePoint;
    [ReadOnly] public GameObject cinemachineFollow;
    [ReadOnly] public Rigidbody rigidBody;
    [ReadOnly] public LineRenderer lineRenderer;
    
    [Header(("Controllers"))]
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;
    [ReadOnly] public GrappleController grappleController;

    public bool isAffectedByMass;
    
    [Header("Movement And Rotation")] 
    public float movementRotationSpeed = 10; 
    public float acceleration = 100;
    public float maxSpeed = 10;
    public float maxSwingSpeed = 20;
    
    [Header("Jumping")]
    public float maxJumpForce;
    public float buttonHoldTime;
    public float floatTime;

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
    
    [Header("External Objects and Components")]
    [ReadOnly] public Transform realCamera;

    [Header("Cinemachine")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    
    [Header("Input Data")]
    [ReadOnly] public Vector2 moveDirection;
    [ReadOnly] public Vector2 cameraLookDelta;
    [ReadOnly] public float cinemachineFollowYaw;
    [ReadOnly] public float cinemachineFollowPitch;
    
    [Header("Aiming Data")]
    [ReadOnly] public bool isAiming;

    [Header("Grapple Data")]
    public float horizontalAirThrust = 20;
    public float verticalAirThrust = 20;
    public Vector3 firstEnd;
    [ReadOnly] public SpringJoint joint;
    [ReadOnly] public FixedJoint fixedJoint;
    public float rayCastDistance;
    public float maxRopeDistance;
    public float minRopeDistance;
    public float springSpringiness;
    public float springDamper;
    public float springMassScale;

    [Header("Rope Data")]
    public GameObject ropePrefab;
    public List<GameObject> ropes = new();
    public GameObject currentRope;

    [Header("Ground Check Data")]
    public float maxSlopeAngle;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isOnSlope;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;

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
            maxJumpForce *= mass;
            horizontalAirThrust *= mass;
            verticalAirThrust *= mass;
        }
    }
}
