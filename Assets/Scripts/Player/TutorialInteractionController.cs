using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;

public class TutorialInteractionController : MonoBehaviour
{
    public static TutorialInteractionController Instance { get; private set; }
    public static readonly ToggleEvent OnToggleAiming = new();
    
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;

    private float _timeElapsedPressed;
    
    private Vector2 _previousMouseDelta;

    public bool isMovementEnabled;
    public bool hasMoved;
    private float _moveDistance;

    public bool isCameraEnabled;
    public bool hasLookedAround;
    private float _lookAroundAmount;

    public bool isJumpEnabled;
    public bool hasJumped;
    public int jumpCount;

    public bool hasExitFirstStage;

    public bool hasEnteredThirdStage;

    public bool isAimingEnabled;
    public bool hasEnteredAimMode;
    public bool hasExitedAimMode;

    public bool isObjectSelectionEnabled;
    public bool hasSelectedObject;

    public bool isSurfaceSelectionEnabled;
    public bool hasSelectedSurface;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        SetupPlayerInput();
        SetupCameraInput();
    }

    private void Start()
    {
        StartCoroutine(TutorialSequence());
    }
    
    IEnumerator TutorialSequence()
    {
        yield return new WaitForSecondsRealtime(3f);

        // Teach movement and camera
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Use WASD to move around and Mouse to move the camera."));
        isMovementEnabled = true;
        isCameraEnabled = true;
        yield return new WaitUntil(() => hasMoved && hasLookedAround);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(3f);

        isJumpEnabled = true;
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Use Space to jump. Try jumping a few times."));
        yield return new WaitUntil(() => hasJumped);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try to get out of the room."));
        yield return new WaitUntil(() => hasExitFirstStage);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());

        yield return new WaitUntil(() => hasEnteredThirdStage);
        
        isMovementEnabled = false;
        isCameraEnabled = false;
        isJumpEnabled = false;
        PlayerData.Instance.moveDirection = Vector2.zero;
        PlayerData.Instance.cameraLookDelta = Vector2.zero;
        PlayerData.Instance.cameraController.SwitchVirtualCamera(2);
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can create elastic ropes from these purple objects."));
        yield return new WaitForSecondsRealtime(3f);
        TutorialPanelController.Instance.ChangeText("You can select objects in aim mode.");
        yield return new WaitForSecondsRealtime(3f);

        PlayerData.Instance.cameraController.SwitchVirtualCamera(0);
        yield return new WaitForSecondsRealtime(.5f);
        isMovementEnabled = true;
        isCameraEnabled = true;
        isJumpEnabled = true;

        TutorialPanelController.Instance.ChangeText("Use Right Click to enter aim mode.");
        isAimingEnabled = true;
        yield return new WaitUntil(() => hasEnteredAimMode);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Time is slowed down when you are in aim mode."));
        yield return new WaitForSecondsRealtime(3f);
        TutorialPanelController.Instance.ChangeText("Try to exit aim mode using Right Click or Q.");
        yield return new WaitUntil(() => hasExitedAimMode);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Let's try selecting objects. Enter aim mode with Right Click, and then Left Click on the object to select it."));
        isObjectSelectionEnabled = true;
        yield return new WaitUntil(() => hasSelectedObject);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
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
        _playerInput.CharacterControls.ToggleRopePlacement.started += HandleToggleRopePlacementInput;
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
        if (!isJumpEnabled) return;
        
        // Don't jump conditions
        if (context.duration > 0.1f) return;
        if (context is { canceled: false, interaction: not HoldInteraction }) return;
        if (PlayerData.Instance.verticalStateController.currentState != PlayerData.Instance.verticalStateController.groundedState) return;

        float jumpPercentage = Mathf.Min((float)context.duration, PlayerData.Instance.buttonHoldTime) / PlayerData.Instance.buttonHoldTime;
        PlayerData.Instance.currentJumpPercentage = jumpPercentage;
        PlayerData.Instance.verticalStateController.Jump();
        
        if (hasJumped) return;
        jumpCount++;
        if (jumpCount > 3) hasJumped = true;
    }
    
    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        if (!isMovementEnabled) return;
        
        PlayerData.Instance.moveDirection = context.ReadValue<Vector2>();

        if (hasMoved) return;
        _moveDistance += context.ReadValue<Vector2>().magnitude;
        if (_moveDistance > 10) hasMoved = true;
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        if (!isCameraEnabled) return; 
        
        PlayerData.Instance.cameraLookDelta = Vector2.Lerp(_previousMouseDelta, context.ReadValue<Vector2>(), PlayerData.Instance.cameraLerpWeight) * (1 / Time.timeScale);
        _previousMouseDelta = context.ReadValue<Vector2>();
        
        if (hasLookedAround) return;
        _lookAroundAmount += context.ReadValue<Vector2>().magnitude;
        if (_lookAroundAmount > 10) hasLookedAround = true;
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
            if (!isObjectSelectionEnabled) return;
            
            PlayerData.Instance.RopePlacementController.SelectObject(hit);
            if (!hasSelectedObject) hasSelectedObject = true;
        }
        
        
        // Add material check, which ones can be attached to which ones cannot
        // hit.collider.gameObject.GetComponent<MeshRenderer>().material.Equals();
        else
        {
            if (!isSurfaceSelectionEnabled) return;
            
            PlayerData.Instance.RopePlacementController.SelectSurface(hit);
            if (!hasSelectedSurface) hasSelectedSurface = true;
        }
    }

    private void HandleToggleRopePlacementInput(InputAction.CallbackContext context)
    {
        if (!isAimingEnabled) return;
        
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        
        if (!PlayerData.Instance.isAiming)
        {
            ToggleAiming(true);

            if (!hasEnteredAimMode) hasEnteredAimMode = true;
        }
        else
        {
            PlayerData.Instance.RopePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            if (!hasExitedAimMode) hasExitedAimMode = true;
        }
    }

    private void HandleDetachLastRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;

        if (PlayerData.Instance.isAiming)
        {
            PlayerData.Instance.RopePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            if (!hasExitedAimMode) hasExitedAimMode = true;
        }
        else
        {
            if (PlayerData.Instance.placedRopes.Count == 0) return;
            PlayerData.Instance.RopePlacementController.DestroyNewestBatch();
        }
    }

    private void HandleDetachFirstRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.placedRopes.Count == 0) return;
            
        PlayerData.Instance.RopePlacementController.DestroyOldestBatch();
    }

    private void HandleConfirmRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton() || !PlayerData.Instance.isAiming) return;

        if (PlayerData.Instance.activeRopes.Count > 0)
        {
            PlayerData.Instance.RopePlacementController.ConfirmPlacement();
            
            ToggleAiming(false, true);
        }
        else
        {
            PlayerData.Instance.RopePlacementController.DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            if (!hasExitedAimMode) hasExitedAimMode = true;
        }
    }

    // private void DestroyRopeBatch(LinkedList<GameObject> linkedList)
    // {
    //     while (linkedList.Count > 0)
    //     {
    //         GameObject ropeObject = linkedList.Last.Value;
    //         Rope rope = ropeObject.GetComponent<Rope>();
    //
    //         if (rope.joint) Destroy(rope.joint);
    //         if (rope.attachmentPoint) Destroy(rope.attachmentPoint);
    //         Destroy(ropeObject);
    //         linkedList.RemoveLast();
    //     }
    // }

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
