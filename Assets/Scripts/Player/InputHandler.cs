using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
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

        _playerInput.CharacterControls.PutEnd.started += HandlePutEndInput;
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
        if (PlayerData.Instance.verticalStateController.currentState != PlayerData.Instance.verticalStateController.groundedState) return;

        float percentage = Mathf.Min((float)context.duration, PlayerData.Instance.buttonHoldTime) / PlayerData.Instance.buttonHoldTime;
        PlayerData.Instance.verticalStateController.Jump(percentage);
    }

    private void HandleGrappleInput(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            PlayerData.Instance.grappleController.StartGrapple();
        }
        
        if (context.canceled)
        {
            PlayerData.Instance.grappleController.StopGrapple();
        }
    }

    private void HandlePutEndInput(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (!PlayerData.Instance.currentRope)
                PlayerData.Instance.currentRope = Instantiate(PlayerData.Instance.ropePrefab);

            Rope rope = PlayerData.Instance.currentRope.GetComponent<Rope>();
            rope.PlaceEnd();

            if (rope.ends.Count > 1) PlayerData.Instance.currentRope = null;
        }
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.moveDirection = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.cameraLookDelta = context.ReadValue<Vector2>();
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
