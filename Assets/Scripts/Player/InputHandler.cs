using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;

    private float _timeElapsedPressed;
    
    // Left click to shoot first rope end
        // Hold to keep attaching it to the player
        // Release to destroy the rope
        // E to shoot second rope end
    // Right click to pick up mechanics
        // Hold to keep holding on mechanics
        // Release to release mechanics
    
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

        _playerInput.CharacterControls.Jump.canceled += HandleJumpInput;
        _playerInput.CharacterControls.Jump.performed += HandleJumpInput;

        _playerInput.CharacterControls.Grapple.started += HandleGrappleInput;
        _playerInput.CharacterControls.Grapple.canceled += HandleGrappleInput;
    }

    private void SetupCameraInput()
    {
        _cameraInput = new CameraInput();
        _cameraInput.CameraLook.Rotate.started += HandleCameraInput;
        _cameraInput.CameraLook.Rotate.performed += HandleCameraInput;
        _cameraInput.CameraLook.Rotate.canceled += HandleCameraInput;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        // Don't jump conditions
        if (context.duration > 0.1f) return;
        if (context is { canceled: false, interaction: not HoldInteraction }) return;
        if (playerData.verticalStateController.currentState != playerData.verticalStateController.groundedState) return;

        float percentage = Mathf.Min((float)context.duration, playerData.buttonHoldTime) / playerData.buttonHoldTime;
        playerData.verticalStateController.Jump(percentage);
    }

    private void HandleGrappleInput(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            playerData.verticalStateController.StartGrapple();
        }

        if (context.canceled)
        {
            // remove grapple
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
