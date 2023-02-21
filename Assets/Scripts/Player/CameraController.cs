using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        InitialSetup();
    }

    private void Update()
    {
        UpdateOrientation();
    }

    private void FixedUpdate()
    {
        RotateCinemachineFollow();
    }

    private void InitialSetup()
    {
        Quaternion cinemachineFollowQuaternion = playerData.cinemachineFollow.transform.rotation;
        playerData.cinemachineFollowYaw = cinemachineFollowQuaternion.eulerAngles.y;
        playerData.cinemachineFollowPitch = cinemachineFollowQuaternion.eulerAngles.x;
    }

    private void UpdateOrientation()
    {
        Vector3 viewDir = gameObject.transform.position - playerData.realCamera.position;
        viewDir.y = 0;
        playerData.orientation.forward = viewDir.normalized;
    }

    private void RotateCinemachineFollow()
    {
        Vector2 cameraDelta = playerData.cameraLookDelta;
        playerData.cinemachineFollowYaw += cameraDelta.x * playerData.cameraRotationSpeed * Time.deltaTime * (playerData.invertX ? -1 : 1);
        playerData.cinemachineFollowPitch += cameraDelta.y * playerData.cameraRotationSpeed * Time.deltaTime * (playerData.invertY ? -1 : 1);

        playerData.cinemachineFollowYaw = ClampAngle(playerData.cinemachineFollowYaw, float.MinValue, float.MaxValue);
        playerData.cinemachineFollowPitch = ClampAngle(playerData.cinemachineFollowPitch, -30f, 70f);

        playerData.cinemachineFollow.transform.rotation = Quaternion.Euler(playerData.cinemachineFollowPitch, playerData.cinemachineFollowYaw, 0f);
    }
    
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SwitchVirtualCamera(int index)
    {
        CinemachineVirtualCameraBase newVirtualCamera = playerData.virtualCameras[index];

        newVirtualCamera.gameObject.SetActive(true);

        foreach (CinemachineVirtualCameraBase virtualCamera in playerData.virtualCameras)
        {
            if (virtualCamera != newVirtualCamera) virtualCamera.gameObject.SetActive(false);
        }

        playerData.currentVirtualCamera = newVirtualCamera;
    }
}
