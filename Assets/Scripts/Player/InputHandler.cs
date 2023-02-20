using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerData = GetComponent<PlayerData>();
        SetupPlayerInput();
        SetupCameraInput();
    }

    private void SetupPlayerInput()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Move.started += HandleMovementInput;
        _playerInput.CharacterControls.Move.performed += HandleMovementInput;
        _playerInput.CharacterControls.Move.canceled += HandleMovementInput;

        _playerInput.CharacterControls.Aim.started += HandleAimInput;
        _playerInput.CharacterControls.Aim.canceled += HandleAimInput;

        _playerInput.CharacterControls.Jump.started += HandleJumpInput;
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
        playerData.isAiming = context.ReadValueAsButton();

        playerData.cameraController.SwitchVirtualCamera(playerData.isAiming ? 1 : 0);
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (playerData.verticalStateController.currentState == playerData.verticalStateController.groundedState && context.ReadValueAsButton())
        {
            playerData.verticalStateController.Jump();
        }
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        playerData.moveDirection = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        playerData.cameraLookDelta = context.ReadValue<Vector2>();
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
