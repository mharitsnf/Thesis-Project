using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ToggleEvent : UnityEvent<bool> {}

public class InteractionController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;

    private float _timeElapsedPressed;

    public static readonly ToggleEvent OnToggleAiming = new();


    // Start is called before the first frame update
    void Awake()
    {
        SetupPlayerInput();
        SetupCameraInput();
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

        _playerInput.CharacterControls.SwitchInteractionState.started += HandleSwitchInteractionStateInput;

        _playerInput.CharacterControls.InteractRopePlacement.started += HandleInteractRopePlacementInput;
        _playerInput.CharacterControls.ToggleRopePlacement.started += HandleToggleRopePlacementInput;
        _playerInput.CharacterControls.ConfirmRopePlacement.started += HandleConfirmRopePlacementInput;
        _playerInput.CharacterControls.DetachLastRopePlacement.started += HandleDetachLastRopePlacementInput;
        _playerInput.CharacterControls.DetachFirstRopePlacement.started += HandleDetachFirstRopePlacementInput;

        _playerInput.CharacterControls.InteractAttaching.started += HandleInteractAttachingInput;
        _playerInput.CharacterControls.ToggleAttaching.started += HandleToggleAttachingInput;
        _playerInput.CharacterControls.DetachAttaching.started += HandleDetachAttachingInput;
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

        PlayerData.Instance.currentJumpPercentage = Mathf.Min((float)context.duration, PlayerData.Instance.buttonHoldTime) / PlayerData.Instance.buttonHoldTime;
        PlayerData.Instance.verticalStateController.Jump();
    }


    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.moveDirection = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        PlayerData.Instance.cameraLookDelta = context.ReadValue<Vector2>() * (1 / Time.timeScale);
    }

    private void HandleSwitchInteractionStateInput(InputAction.CallbackContext context)
    {
        // if (!context.ReadValueAsButton()) return;
        // if (PlayerData.Instance.isAiming) return;
        //
        // PlayerData.Instance.currentInteractionState = PlayerData.Instance.currentInteractionState == PlayerData.InteractionState.Attaching ? PlayerData.InteractionState.RopePlacement : PlayerData.InteractionState.Attaching;
        //
        // Debug.Log(PlayerData.Instance.currentInteractionState);
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
            if (!hit.collider.gameObject.CompareTag("Object")) return;
            PlayerData.Instance.selectedGameObject = hit;
            hit.collider.gameObject.GetComponentInChildren<ObjectMechanicsController>().SetSelected();

            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.ObjectSelected;
        }
        else
        {
            if (PlayerData.Instance.selectedGameObject.collider.gameObject.Equals(hit.collider.gameObject)) return;
            
            PlayerData.Instance.activeRopes.AddLast(Instantiate(PlayerData.Instance.ropePrefab));
            
            GameObject lastRopeObject = PlayerData.Instance.TryPeekActiveRope();
            Rope lastRope = lastRopeObject.GetComponent<Rope>();
            lastRope.PlaceEnd(PlayerData.Instance.selectedGameObject);

            lastRope.PlaceEnd(hit);
        }
    }

    private void HandleToggleRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        
        if (!PlayerData.Instance.isAiming)
        {
            ToggleAiming(true);

            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.ObjectNotSelected;
        }
        else
        {
            DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        }
    }

    private void HandleDetachLastRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;

        if (PlayerData.Instance.isAiming)
        {
            DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        }
        else
        {
            if (PlayerData.Instance.placedRopes.Count == 0) return;
            
            LinkedList<GameObject> previousRopeStack = PlayerData.Instance.placedRopes.Last.Value;
            PlayerData.Instance.placedRopes.RemoveLast();
            DestroyRopeBatch(previousRopeStack);
            
            UpdateRopeColor(false);

            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        }
    }

    private void HandleDetachFirstRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton()) return;
        if (PlayerData.Instance.isAiming) return;
        if (PlayerData.Instance.placedRopes.Count == 0) return;
            
        LinkedList<GameObject> previousRopeStack = PlayerData.Instance.placedRopes.First.Value;
        PlayerData.Instance.placedRopes.RemoveFirst();
        DestroyRopeBatch(previousRopeStack);
        
        UpdateRopeColor(false);
            
        InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        
    }

    private void HandleConfirmRopePlacementInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.RopePlacement) return;
        if (!context.ReadValueAsButton() || !PlayerData.Instance.isAiming) return;

        if (PlayerData.Instance.activeRopes.Count > 0)
        {
            UpdateRopeColor(true);
                
            foreach (var rope in PlayerData.Instance.activeRopes.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                rope.CreateJoint();
            }

            PlayerData.Instance.placedRopes.AddLast(new LinkedList<GameObject>(PlayerData.Instance.activeRopes));
            PlayerData.Instance.activeRopes.Clear();

            ToggleAiming(false, true);
            
            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        }
        else
        {
            DestroyRopeBatch(PlayerData.Instance.activeRopes);
            ToggleAiming(false);
            
            InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
        }
    }

    private void HandleInteractAttachingInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.Attaching) return;
        if (!PlayerData.Instance.isAiming) return;
        if (!context.ReadValueAsButton()) return;
        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.attachRaycastDistance)) return;
        if (hit.collider.CompareTag("Player")) return;
        if (!hit.rigidbody) return;

        
        FixedJoint joint = PlayerData.Instance.fixedJoint ? gameObject.GetComponent<FixedJoint>() : gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hit.rigidbody;
        if (!PlayerData.Instance.fixedJoint) PlayerData.Instance.fixedJoint = PlayerData.Instance.fixedJoint = joint;

        ToggleAiming(false);
    }
    
    private void HandleToggleAttachingInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.Attaching) return;
        if (!context.ReadValueAsButton()) return;

        ToggleAiming(!PlayerData.Instance.isAiming);
    }

    private void HandleDetachAttachingInput(InputAction.CallbackContext context)
    {
        if (PlayerData.Instance.currentInteractionState != PlayerData.InteractionState.Attaching) return;
        if (!context.ReadValueAsButton()) return;
        if (!PlayerData.Instance.fixedJoint) return;

        ToggleAiming(false);

        Destroy(PlayerData.Instance.fixedJoint);
        PlayerData.Instance.fixedJoint = null;
    }

    private void DestroyRopeBatch(LinkedList<GameObject> linkedList)
    {
        while (linkedList.Count > 0)
        {
            GameObject ropeObject = linkedList.Last.Value;
            Rope rope = ropeObject.GetComponent<Rope>();

            if (rope.joint) Destroy(rope.joint);
            if (rope.attachmentPoint) Destroy(rope.attachmentPoint);
            Destroy(ropeObject);
            linkedList.RemoveLast();
        }
    }

    private void UpdateRopeColor(bool inserted)
    {
        if (PlayerData.Instance.placedRopes.First != null)
        {
            foreach (var rope in PlayerData.Instance.placedRopes.First.Value.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                rope.SetColor(1);
            }
        }
            
        if (PlayerData.Instance.placedRopes.Last != null)
        {
            foreach (var rope in PlayerData.Instance.placedRopes.Last.Value.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                rope.SetColor(inserted ? 0 : 2);
            }
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
        }
        else
        {
            PlayerData.Instance.isAiming = false;
            Time.timeScale = 1;
            PlayerData.Instance.cameraController.SwitchVirtualCamera(0);
    
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
