using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    public Vector2 moveDirection;
    public Vector2 cameraLookDelta;

    public bool isAiming;

    private PlayerInput _playerInput;
    private CameraInput _cameraInput;
    private CinemachineFollowController _cinemachineFollowController;
    
    // Start is called before the first frame update
    void Awake()
    {
        SetupPlayerInput();
        SetupCameraInput();
    }

    private void Start()
    {
        _cinemachineFollowController = GetComponent<CinemachineFollowController>();
    }

    private void SetupPlayerInput()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Move.started += HandleMovementInput;
        _playerInput.CharacterControls.Move.performed += HandleMovementInput;
        _playerInput.CharacterControls.Move.canceled += HandleMovementInput;

        _playerInput.CharacterControls.Aim.started += HandleAimInput;
        _playerInput.CharacterControls.Aim.canceled += HandleAimInput;
    }

    private void SetupCameraInput()
    {
        _cameraInput = new CameraInput();
        _cameraInput.CameraLook.Rotate.started += HandleCameraInput;
        _cameraInput.CameraLook.Rotate.performed += HandleCameraInput;
        _cameraInput.CameraLook.Rotate.canceled += HandleCameraInput;
    }

    private void HandleAimInput(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
        if (isAiming) _cinemachineFollowController.SwitchCamera(_cinemachineFollowController.virtualCameras[1]);
        else _cinemachineFollowController.SwitchCamera(_cinemachineFollowController.virtualCameras[0]);
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        cameraLookDelta = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _cameraInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _cameraInput.Disable();
    }
}
