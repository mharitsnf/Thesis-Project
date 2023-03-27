using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ToggleEvent : UnityEvent<bool> {}

public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance { get; private set; }
    public static readonly ToggleEvent OnToggleAiming = new();
    
    public PlayerInput playerInput;
    public CameraInput cameraInput;

    private float _timeElapsedPressed;
    
    private Vector2 _previousMouseDelta;
    
    private float _moveDistance;
    private float _lookAroundAmount;
    private int _jumpCount;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        SetupPlayerInput();
        SetupCameraInput();
    }

    private void Update()
    {
        PlayerData.Instance.cameraLookDelta = Vector2.Lerp(PlayerData.Instance.cameraLookDelta, Vector2.zero, PlayerData.Instance.cameraBrakeWeight * Time.deltaTime * (1 / Time.timeScale));
    }

    private void OnEnable()
    {
        // if (SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        //
        // playerInput.Enable();
        // playerInput.CharacterControls.ExitAim.Disable();
        // cameraInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        cameraInput.Disable();
    }
    
    private void SetupPlayerInput()
    {
        playerInput = new PlayerInput();
        playerInput.CharacterControls.Move.started += HandleMovementInput;
        playerInput.CharacterControls.Move.performed += HandleMovementInput;
        playerInput.CharacterControls.Move.canceled += HandleMovementInput;

        playerInput.CharacterControls.Crouch.started += HandleCrouchInput;
        playerInput.CharacterControls.Crouch.canceled += HandleCrouchInput;

        playerInput.CharacterControls.Jump.canceled += HandleJumpInput;
        playerInput.CharacterControls.Jump.performed += HandleJumpInput;
        
        playerInput.CharacterControls.EnterAim.started += HandleEnterAimInput;
        playerInput.CharacterControls.ExitAim.started += HandleExitAimInput;

        playerInput.CharacterControls.SelectObject.started += HandleSelectObjectInput;
        playerInput.CharacterControls.SelectSurface.started += HandleSelectSurfaceInput;
        
        playerInput.CharacterControls.ConfirmRopePlacement.started += HandleConfirmRopePlacementInput;
        playerInput.CharacterControls.DetachLastRopePlacement.started += HandleDetachLastRopePlacementInput;
        playerInput.CharacterControls.DetachFirstRopePlacement.started += HandleDetachFirstRopePlacementInput;

        playerInput.OtherInteraction.NextStage.started += HandleNextStageInput;
    }

    private void SetupCameraInput()
    {
        cameraInput = new CameraInput();
        cameraInput.CameraLook.Rotate.performed += HandleCameraInput;
    }
    
    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        // Don't jump conditions
        if (context is { interaction: not HoldInteraction }) return;
        if (context.canceled && context.duration > PlayerData.Instance.buttonHoldTime) return;

        float jumpPercentage = Mathf.Min((float)context.duration, PlayerData.Instance.buttonHoldTime) / PlayerData.Instance.buttonHoldTime;
        PlayerData.Instance.currentJumpPercentage = (Mathf.Pow(jumpPercentage, PlayerData.Instance.jumpExponent) - Mathf.Pow(0, PlayerData.Instance.jumpExponent) /
                                                    (Mathf.Pow(1, PlayerData.Instance.jumpExponent) - Mathf.Pow(0, PlayerData.Instance.jumpExponent)));
        PlayerData.Instance.verticalStateController.Jump();
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (TutorialCore.Instance.hasJumped) return;
        _jumpCount++;
        if (_jumpCount > 3) TutorialCore.Instance.hasJumped = true;
    }
    
    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.moveDirection = context.ReadValue<Vector2>();

        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (TutorialCore.Instance.hasMoved) return;
        _moveDistance += context.ReadValue<Vector2>().magnitude;
        if (_moveDistance > 10) TutorialCore.Instance.hasMoved = true;
    }

    private void HandleCrouchInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.IsCrouching = context.ReadValueAsButton();
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasCrouched) TutorialCore.Instance.hasCrouched = true;
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.cameraLookDelta = Vector2.Lerp(_previousMouseDelta, context.ReadValue<Vector2>(), PlayerData.Instance.cameraLerpWeight) * (1 / Time.timeScale);
        _previousMouseDelta = context.ReadValue<Vector2>();
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (TutorialCore.Instance.hasLookedAround) return;
        _lookAroundAmount += context.ReadValue<Vector2>().magnitude;
        if (_lookAroundAmount > 10) TutorialCore.Instance.hasLookedAround = true;
    }

    private void HandleEnterAimInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;

        ToggleAiming(true);
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.ObjectNotSelected;

        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasEnteredAimMode) TutorialCore.Instance.hasEnteredAimMode = true;
    }

    private void HandleExitAimInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (!PlayerData.Instance.isAiming) return;

        PlayerData.Instance.ropePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
        ToggleAiming(false);
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
            
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasExitedAimMode) TutorialCore.Instance.hasExitedAimMode = true;
    }

    private void HandleSelectObjectInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!PlayerData.Instance.isAiming) return;
        if (!context.ReadValueAsButton()) return;
        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.ropeRayCastDistance)) return;
        if (hit.collider.CompareTag("Player")) return;
        if (!PlayerData.Instance.selectedGameObject.Equals(default(RaycastHit))) return;
        if (!hit.collider.gameObject.CompareTag("Object")) return;

        PlayerData.Instance.ropePlacementController.SelectObject(hit);
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.ObjectSelected;
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasSelectedObject) TutorialCore.Instance.hasSelectedObject = true;
    }
    
    private void HandleSelectSurfaceInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!PlayerData.Instance.isAiming) return;
        if (!context.ReadValueAsButton()) return;
        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.ropeRayCastDistance, PlayerData.Instance.selectableSurfaces)) return;
        if (hit.collider.CompareTag("Player")) return;
        if (hit.collider.gameObject.layer == 9) return;
        if (PlayerData.Instance.selectedGameObject.Equals(default(RaycastHit))) return;
        if (PlayerData.Instance.selectedGameObject.collider.gameObject.Equals(hit.collider.gameObject)) return;

        PlayerData.Instance.ropePlacementController.SelectSurface(hit);
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasSelectedSurface) TutorialCore.Instance.hasSelectedSurface = true;
    }

    private void HandleDetachLastRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.placedRopes.Count == 0) return;

        PlayerData.Instance.ropePlacementController.DestroyNewestBatch();
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
    
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasDetachedLast) TutorialCore.Instance.hasDetachedLast = true;
    }

    private void HandleDetachFirstRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.placedRopes.Count == 0) return;
            
        PlayerData.Instance.ropePlacementController.DestroyOldestBatch();
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;

        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        if (!TutorialCore.Instance.hasDetachedFirst) TutorialCore.Instance.hasDetachedFirst = true;
    }

    private void HandleConfirmRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton() || !PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.activeRopes.Count <= 0) return;

        PlayerData.Instance.ropePlacementController.ConfirmPlacement();
            
        ToggleAiming(false, true);
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) return;
        TutorialCore.Instance.ropeBatchCount++;
        if (!TutorialCore.Instance.hasConfirmedSelection) TutorialCore.Instance.hasConfirmedSelection = true;
    }

    private void HandleNextStageInput(InputAction.CallbackContext context)
    {
        LevelLoader.Instance.LoadNextLevel();
    }

    public void ForceQuitAiming()
    {
        PlayerData.Instance.ropePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
        ToggleAiming(false);
        
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
    }

    private void ToggleAiming(bool isAiming, bool isJointPlaced = false)
    {
        OnToggleAiming.Invoke(isAiming);
        
        if (isAiming)
        {
            PlayerData.Instance.isAiming = true;
            PlayerData.Instance.cameraController.SwitchVirtualCamera(PlayerData.Instance.virtualCameras[1]);
            Time.timeScale = PlayerData.Instance.aimingTimeScale;
            
            PlayerData.Instance.crosshairObject.SetActive(true);
            
            playerInput.CharacterControls.ExitAim.Enable();
            playerInput.CharacterControls.EnterAim.Disable();
        }
        else
        {
            PlayerData.Instance.isAiming = false;
            Time.timeScale = 1;
            PlayerData.Instance.cameraController.SwitchVirtualCamera(PlayerData.Instance.virtualCameras[0]);
            
            PlayerData.Instance.crosshairObject.SetActive(false);
            
            playerInput.CharacterControls.ExitAim.Disable();
            playerInput.CharacterControls.EnterAim.Enable();

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
