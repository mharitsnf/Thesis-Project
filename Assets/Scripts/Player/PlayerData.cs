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
    [Header("Player Objects and Components")]
    [ReadOnly] public Transform meshes;
    [ReadOnly] public Transform orientation;
    [ReadOnly] public Transform grapplePoint;
    [ReadOnly] public Rigidbody rigidBody;
    [ReadOnly] public GameObject cinemachineFollow;
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;
    [ReadOnly] public CameraController cameraController;
    [ReadOnly] public LineRenderer lineRenderer;
    
    [Header("Movement And Rotation")] 
    public float movementRotationSpeed = 10; 
    public float acceleration = 100;
    public float maxSpeed = 10;
    
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
    public bool invertY;
    public bool invertX;
    
    [Header("External Objects and Components")]
    [ReadOnly] public Transform realCamera;

    [Header("Cinemachine")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    [ReadOnly] public CinemachineVirtualCameraBase currentVirtualCamera;
    
    [Header("Input Data")]
    [ReadOnly] public Vector2 moveDirection;
    [ReadOnly] public Vector2 cameraLookDelta;
    [ReadOnly] public float cinemachineFollowYaw;
    [ReadOnly] public float cinemachineFollowPitch;
    
    [Header("Aiming Data")]
    [ReadOnly] public bool isAiming;

    [Header("Rope Data")]
    public Vector3 firstEnd;
    [ReadOnly] public SpringJoint joint;
    public float maxRopeDistance;

    [Header("Ground Check Data")]
    public float maxSlopeAngle;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isOnSlope;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;
}
