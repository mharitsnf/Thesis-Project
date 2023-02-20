using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    [HideInInspector] public CinemachineVirtualCameraBase currentVirtualCamera;

    private void SwitchCamera(CinemachineVirtualCameraBase virtualCamera)
    {
        virtualCamera.Priority = 10;

        foreach (CinemachineVirtualCameraBase vc in virtualCameras)
        {
            if (vc != virtualCamera) vc.Priority = 0;
        }

        currentVirtualCamera = virtualCamera;
    }
}
