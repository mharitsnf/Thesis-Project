using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineFollowController : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject cinemachineFollow;

    public float rotationSpeed;
    public bool invertY;
    public bool invertX;

    public CinemachineVirtualCameraBase[] virtualCameras;
    private CinemachineVirtualCameraBase _currentVirtualCamera;

    private float _cinemachineFollowYaw;
    private float _cinemachineFollowPitch;

    private void Start()
    {
        Quaternion cinemachineFollowQuaternion = cinemachineFollow.transform.rotation;
        _cinemachineFollowYaw = cinemachineFollowQuaternion.eulerAngles.y;
        _cinemachineFollowPitch = cinemachineFollowQuaternion.eulerAngles.x;
    }

    private void FixedUpdate()
    {
        RotateCinemachineFollow();
    }

    private void RotateCinemachineFollow()
    {
        Vector2 cameraDelta = inputHandler.cameraLookDelta;
        _cinemachineFollowYaw += cameraDelta.x * rotationSpeed * Time.deltaTime * (invertX ? -1 : 1);
        _cinemachineFollowPitch += cameraDelta.y * rotationSpeed * Time.deltaTime * (invertY ? -1 : 1);

        _cinemachineFollowYaw = ClampAngle(_cinemachineFollowYaw, float.MinValue, float.MaxValue);
        _cinemachineFollowPitch = ClampAngle(_cinemachineFollowPitch, -30f, 70f);

        cinemachineFollow.transform.rotation = Quaternion.Euler(_cinemachineFollowPitch, _cinemachineFollowYaw, 0f);
    }
    
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SwitchCamera(int index)
    {
        CinemachineVirtualCameraBase newVirtualCamera = virtualCameras[index];
        
        newVirtualCamera.gameObject.SetActive(true);

        foreach (CinemachineVirtualCameraBase virtualCamera in virtualCameras)
        {
            if (virtualCamera != newVirtualCamera) virtualCamera.gameObject.SetActive(false);
        }

        _currentVirtualCamera = newVirtualCamera;
    }
}
