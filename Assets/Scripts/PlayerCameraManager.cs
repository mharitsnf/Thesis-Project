using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCameraBase[] virtualCameras;
    [HideInInspector] public CinemachineVirtualCameraBase currentVirtualCamera;

    // Player input
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Aim.started += HandleAimInput;
        _playerInput.CharacterControls.Aim.canceled += HandleAimInput;
    }

    private void Start()
    {
        SwitchCamera(virtualCameras[1]);
    }

    private void SwitchCamera(CinemachineVirtualCameraBase virtualCamera)
    {
        virtualCamera.Priority = 10;

        foreach (CinemachineVirtualCameraBase vc in virtualCameras)
        {
            if (vc != virtualCamera) vc.Priority = 0;
        }

        currentVirtualCamera = virtualCamera;
    }

    private void HandleAimInput(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton()) SwitchCamera(virtualCameras[0]);
        else SwitchCamera(virtualCameras[1]);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
