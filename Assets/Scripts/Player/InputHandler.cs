using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CameraInput _cameraInput;

    private float _timeElapsedPressed;
    

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

        _playerInput.CharacterControls.Interact.started += HandleInteractInput;

        _playerInput.CharacterControls.ToggleRopePlacement.started += HandleToggleRopePlacementInput;

        _playerInput.CharacterControls.ConfirmRopePlacement.started += HandleConfirmRopePlacementInput;

        _playerInput.CharacterControls.DetachRopePlacement.started += HandleDetachRopePlacementInput;
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
        PlayerData.Instance.cameraLookDelta = context.ReadValue<Vector2>();
    }

    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        if (!PlayerData.Instance.isSelecting) return;

        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.rayCastDistance)) return;
        
        if (hit.collider.CompareTag("Player")) return;

        if (PlayerData.Instance.selectedGameObject.Equals(default(RaycastHit)))
        {
            if (!hit.collider.gameObject.CompareTag("Object")) return;
            PlayerData.Instance.selectedGameObject = hit;
        }
        else
        {
            if (PlayerData.Instance.selectedGameObject.Equals(hit)) return;
            
            PlayerData.Instance.activeRopes.Push(Instantiate(PlayerData.Instance.ropePrefab));
            GameObject lastRopeObject = PlayerData.Instance.TryPeekActiveRope();
            Rope lastRope = lastRopeObject.GetComponent<Rope>();
            lastRope.PlaceEnd(PlayerData.Instance.selectedGameObject);

            bool status = lastRope.PlaceEnd(hit);
            Debug.Log(status);
        }
    }

    private void HandleToggleRopePlacementInput(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        
        if (!PlayerData.Instance.isSelecting)
        {
            PlayerData.Instance.isSelecting = true;
            Debug.Log("start selecting");
        }
        else
        {
            while (PlayerData.Instance.activeRopes.Count > 0)
            {
                GameObject ropeObject = PlayerData.Instance.activeRopes.Pop();
                Rope rope = ropeObject.GetComponent<Rope>();
                
                Destroy(rope.joint);
                Destroy(ropeObject);
            }

            PlayerData.Instance.isSelecting = false;
            PlayerData.Instance.selectedGameObject = new RaycastHit();

            Debug.Log("stop selecting");
        }
    }

    private void HandleDetachRopePlacementInput(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;

        if (PlayerData.Instance.isSelecting)
        {
            while (PlayerData.Instance.activeRopes.Count > 0)
            {
                GameObject ropeObject = PlayerData.Instance.activeRopes.Pop();
                Rope rope = ropeObject.GetComponent<Rope>();
                
                Destroy(rope.joint);
                Destroy(ropeObject);
            }

            PlayerData.Instance.isSelecting = false;
            PlayerData.Instance.selectedGameObject = new RaycastHit();

            Debug.Log("stop selecting");
        }
        else
        {
            if (PlayerData.Instance.placedRopes.Count == 0) return;
            
            Stack<GameObject> previousRopes = PlayerData.Instance.placedRopes.Pop();

            Debug.Log(previousRopes.Count);
            
            while (previousRopes.Count > 0)
            {
                GameObject ropeObject = previousRopes.Pop();
                Rope rope = ropeObject.GetComponent<Rope>();

                Debug.Log(ropeObject);
                
                Destroy(rope.joint);
                Destroy(ropeObject);
            }
            
            Debug.Log("remove previous batch");
        }
        
    }

    private void HandleConfirmRopePlacementInput(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton() || !PlayerData.Instance.isSelecting) return;

        PlayerData.Instance.placedRopes.Push(new Stack<GameObject>(PlayerData.Instance.activeRopes));
        PlayerData.Instance.activeRopes.Clear();

        PlayerData.Instance.isSelecting = false;
        PlayerData.Instance.selectedGameObject = new RaycastHit();
        
        Debug.Log("selection confirmed");

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
