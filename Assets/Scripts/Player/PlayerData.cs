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
    [ReadOnly] public Rigidbody rigidBody;
    [ReadOnly] public GameObject cinemachineFollow;
    [ReadOnly] public InputHandler inputHandler;
    [ReadOnly] public VerticalStateController verticalStateController;
    [ReadOnly] public HorizontalStateController horizontalStateController;
    [ReadOnly] public CameraController cameraController;
    
    [Header("External Objects and Components")]
    [ReadOnly] public Transform realCamera;
    
    
    [Header("Cinemachine")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    [ReadOnly] public CinemachineVirtualCameraBase currentVirtualCamera;
    
    [Header("Input Data")]
    [ReadOnly] public Vector2 moveDirection;
    [ReadOnly] public Vector2 lookDirection;
    [ReadOnly] public Vector2 cameraLookDelta;
    [ReadOnly] public float cinemachineFollowYaw;
    [ReadOnly] public float cinemachineFollowPitch;
    
    [Header("Aiming Data")]
    [ReadOnly] public bool isAiming;

    [Header("Ground Check Data")]
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public float playerYCenter;
    [ReadOnly] public RaycastHit groundInfo;
}
