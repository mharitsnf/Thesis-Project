using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;


public class FullInteractionController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;

    private float _timeElapsedPressed;
    
    private Vector2 _previousMouseDelta;

    public static readonly ToggleEvent OnToggleAiming = new();


    // Start is called before the first frame update
    void Awake()
    {
        SetupPlayerInput();
        SetupCameraInput();
    }

    private void Update()
    {
        PlayerData.Instance.cameraLookDelta = Vector2.Lerp(PlayerData.Instance.cameraLookDelta, Vector2.zero, PlayerData.Instance.cameraBrakeWeight * Time.deltaTime * (1 / Time.timeScale));
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

    private void SetupPlayerInput()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Move.started += HandleMovementInput;
        _playerInput.CharacterControls.Move.performed += HandleMovementInput;
        _playerInput.CharacterControls.Move.canceled += HandleMovementInput;

        _playerInput.CharacterControls.Jump.canceled += HandleJumpInput;
        _playerInput.CharacterControls.Jump.performed += HandleJumpInput;

        _playerInput.CharacterControls.InteractRopePlacement.started += HandleInteractRopePlacementInput;
        _playerInput.CharacterControls.EnterAim.started += HandleEnterAimInput;
        _playerInput.CharacterControls.ExitAim.started += HandleExitAimInput;
        _playerInput.CharacterControls.ConfirmRopePlacement.started += HandleConfirmRopePlacementInput;
        _playerInput.CharacterControls.DetachLastRopePlacement.started += HandleDetachLastRopePlacementInput;
        _playerInput.CharacterControls.DetachFirstRopePlacement.started += HandleDetachFirstRopePlacementInput;
    }

    private void SetupCameraInput()
    {
        _cameraInput = new CameraInput();
        _cameraInput.CameraLook.Rotate.performed += HandleCameraInput;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        // Don't jump conditions
        if (context.duration > 0.1f) return;
        if (context is { canceled: false, interaction: not HoldInteraction }) return;
        if (PlayerData.Instance.verticalStateController.currentState != PlayerData.Instance.verticalStateController.groundedState) return;

        float jumpPercentage = Mathf.Min((float)context.duration, PlayerData.Instance.buttonHoldTime) / PlayerData.Instance.buttonHoldTime;
        PlayerData.Instance.currentJumpPercentage = jumpPercentage;
        PlayerData.Instance.verticalStateController.Jump();
    }
    
    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.moveDirection = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.cameraLookDelta = Vector2.Lerp(_previousMouseDelta, context.ReadValue<Vector2>(), PlayerData.Instance.cameraLerpWeight) * (1 / Time.timeScale);
        _previousMouseDelta = context.ReadValue<Vector2>();
    }

    private void HandleInteractRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!PlayerData.Instance.isAiming) return;
        if (!context.ReadValueAsButton()) return;
        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.ropeRayCastDistance)) return;
        if (hit.collider.CompareTag("Player")) return;

        
        if (PlayerData.Instance.selectedGameObject.Equals(default(RaycastHit)))
        {
            PlayerData.Instance.ropePlacementController.SelectObject(hit);
            
            InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.ObjectSelected;
        }
        
        
        // Add material check, which ones can be attached to which ones cannot
        // hit.collider.gameObject.GetComponent<MeshRenderer>().material.Equals();
        else PlayerData.Instance.ropePlacementController.SelectSurface(hit);
    }

    private void HandleEnterAimInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        
        ToggleAiming(true);

        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.ObjectNotSelected;
    }

    private void HandleExitAimInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (!PlayerData.Instance.isAiming) return;
        
        PlayerData.Instance.ropePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
        ToggleAiming(false);
            
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
    }

    private void HandleDetachLastRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;

        if (PlayerData.Instance.isAiming)
        {
            PlayerData.Instance.ropePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        }
        else
        {
            if (PlayerData.Instance.placedRopes.Count == 0) return;
            PlayerData.Instance.ropePlacementController.DestroyNewestBatch();
            
            InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        }
    }

    private void HandleDetachFirstRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.placedRopes.Count == 0) return;
            
        PlayerData.Instance.ropePlacementController.DestroyOldestBatch();
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
    }

    private void HandleConfirmRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton() || !PlayerData.Instance.isAiming) return;

        if (PlayerData.Instance.activeRopes.Count > 0)
        {
            PlayerData.Instance.ropePlacementController.ConfirmPlacement();
            
            ToggleAiming(false, true);
            
            InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        }
        else
        {
            PlayerData.Instance.ropePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        }
    }

    private void ToggleAiming(bool isAiming, bool isJointPlaced = false)
    {
        OnToggleAiming.Invoke(isAiming);
        
        if (isAiming)
        {
            PlayerData.Instance.isAiming = true;
            PlayerData.Instance.cameraController.SwitchVirtualCamera(1);
            Time.timeScale = PlayerData.Instance.aimingTimeScale;
            
            PlayerData.Instance.crosshairObject.SetActive(true);
        }
        else
        {
            PlayerData.Instance.isAiming = false;
            Time.timeScale = 1;
            PlayerData.Instance.cameraController.SwitchVirtualCamera(0);
            
            PlayerData.Instance.crosshairObject.SetActive(false);

            if (PlayerData.Instance.currentInteractionState == PlayerData.InteractionState.RopePlacement)
            {
                if (isJointPlaced)
                {
                    PlayerData.Instance.selectedGameObject.collider.gameObject
                        .GetComponentInChildren<ObjectMechanicsController>().SetMaterial(3);
                    PlayerData.Instance.selectedGameObject.collider.gameObject
                        .GetComponentInChildren<ObjectMechanicsController>().PlayParticle();
                }
                
                PlayerData.Instance.selectedGameObject = new RaycastHit();
            }
        }
    
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
