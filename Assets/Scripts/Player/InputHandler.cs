using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 cameraLookDelta;
    public Vector2 moveDirection;
    public bool isAiming;

    private PlayerHorizontalStateManager _horizontalManager;
    private PlayerVerticalStateManager _verticalManager;
    
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
        _horizontalManager = GetComponent<PlayerHorizontalStateManager>();
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
        
        if (isAiming) _cinemachineFollowController.SwitchCamera(1);
        else _cinemachineFollowController.SwitchCamera(0);
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        _verticalManager.isJumping = context.ReadValueAsButton();
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
