using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialCore : LevelCore
{
    public static TutorialCore Instance { get; private set; }

    public float lerpSmoothing;
    public Transform firstRoomLedge;
    private readonly float _ledgeTargetHeight = 46.25019f;
    
    public Transform thirdRoomDoor;
    private readonly float _doorTargetHeight = 58.75024f;
    
    [Header("Tutorial Flags")]
    public bool hasMoved;
    private float _moveDistance;

    public bool hasLookedAround;
    private float _lookAroundAmount;

    public bool hasJumped;

    public bool hasExitFirstStage;

    public bool hasEnteredThirdStage;

    public bool hasEnteredAimMode;
    public bool hasExitedAimMode;

    public bool hasSelectedObject;

    public bool hasSelectedSurface;

    public bool hasConfirmedSelection;

    public int ropeBatchCount;

    public bool hasDetachedFirst;
    public bool hasDetachedLast;

    public bool hasExitedThirdStage;

    public bool hasEnteredCrouchStage;
    public bool hasCrouched;
    public bool hasExitedCrouchStage;

    public bool hasCompleted;
    

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        StartCoroutine(TutorialSequence());
    }

    IEnumerator MoveFirstLedge()
    {
        Vector3 position = firstRoomLedge.position;
        
        while (position.y > _ledgeTargetHeight)
        {
            position.y = Mathf.Lerp(position.y, _ledgeTargetHeight - 0.1f, lerpSmoothing * Time.deltaTime);
            firstRoomLedge.position = position;

            yield return null;
        }
    }

    IEnumerator MoveThirdStageDoor()
    {
        Vector3 position = thirdRoomDoor.position;
        
        while (position.y > _doorTargetHeight)
        {
            position.y = Mathf.Lerp(position.y, _doorTargetHeight - 0.1f, lerpSmoothing * Time.deltaTime);
            thirdRoomDoor.position = position;

            yield return null;
        }
    }

    IEnumerator TutorialSequence()
    {

        InteractionController.Instance.playerInput.OtherInteraction.NextStage.Enable();
        InteractionController.Instance.playerInput.OtherInteraction.ReloadStage.Enable();

        InstructionGroupController.Instance.IsShown = false;
        yield return new WaitForSecondsRealtime(3f);

        InteractionController.Instance.playerInput.CharacterControls.Disable();
        InteractionController.Instance.cameraInput.Disable();

        // Teach movement and camera
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Use WASD to move around and Mouse to move the camera."));
        InteractionController.Instance.playerInput.CharacterControls.Move.Enable();
        InteractionController.Instance.cameraInput.Enable();
        yield return new WaitUntil(() => hasMoved && hasLookedAround);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1.5f);

        yield return StartCoroutine(PanelController.Instance.ShowPanel("Use Space to jump. Try jumping a few times."));
        InteractionController.Instance.playerInput.CharacterControls.Jump.Enable();
        yield return new WaitUntil(() => hasJumped);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to get out of the room."));
        StartCoroutine(MoveFirstLedge());
        yield return new WaitUntil(() => hasExitFirstStage);
        yield return StartCoroutine(PanelController.Instance.HidePanel());

        yield return new WaitUntil(() => hasEnteredThirdStage);
        
        InteractionController.Instance.playerInput.CharacterControls.Move.Disable();
        InteractionController.Instance.cameraInput.CameraLook.Rotate.Disable();
        InteractionController.Instance.playerInput.CharacterControls.Jump.Disable();
        PlayerData.Instance.moveDirection = Vector2.zero;
        PlayerData.Instance.cameraLookDelta = Vector2.zero;
        PlayerData.Instance.cameraController.SwitchVirtualCamera(PlayerData.Instance.virtualCameras[2]);
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can create elastic ropes from these purple objects.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can select objects in aim mode.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());

        PlayerData.Instance.cameraController.SwitchVirtualCamera(PlayerData.Instance.virtualCameras[0]);
        yield return new WaitForSecondsRealtime(.5f);
        InteractionController.Instance.playerInput.CharacterControls.Move.Enable();
        InteractionController.Instance.cameraInput.CameraLook.Rotate.Enable();
        InteractionController.Instance.playerInput.CharacterControls.Jump.Enable();

        yield return StartCoroutine(PanelController.Instance.ShowPanel("Use Right Click to enter aim mode."));
        InteractionController.Instance.playerInput.CharacterControls.EnterAim.Enable();
        yield return new WaitUntil(() => hasEnteredAimMode);
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Time is slowed down when you are in aim mode.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to exit aim mode using Right Click or Q."));
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Enable();
        yield return new WaitUntil(() => hasExitedAimMode);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(PanelController.Instance.ShowPanel("Let's try selecting objects. Enter aim mode with Right Click, and then Left Click on the object to select it."));
        InteractionController.Instance.playerInput.CharacterControls.SelectObject.Enable();
        yield return new WaitUntil(() => hasSelectedObject);
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Great! Now select anything other than the object."));
        InteractionController.Instance.playerInput.CharacterControls.SelectSurface.Enable();
        yield return new WaitUntil(() => hasSelectedSurface);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can create points on any surface, including other purple objects, but not the white walls.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can have as many points as you'd like too.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try confirming your selection with R."));
        InteractionController.Instance.playerInput.CharacterControls.ConfirmRopePlacement.Enable();
        yield return new WaitUntil(() => hasConfirmedSelection);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can also cancel the points by exiting aim mode.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try repeating the process two more times. Remember to aim at the object first and to confirm with R."));
        yield return new WaitUntil(() => ropeBatchCount > 2);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Ropes comes in different colors: red are the oldest, green are the newest, yellow are anything in between.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can destroy red or green ropes.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try destroying the oldest/red ropes using Q."));
        InteractionController.Instance.playerInput.CharacterControls.DetachFirstRopePlacement.Enable();
        yield return new WaitUntil(() => hasDetachedFirst);
        InteractionController.Instance.playerInput.CharacterControls.DetachFirstRopePlacement.Disable();
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Now try destroying newest/green ropes using E"));
        InteractionController.Instance.playerInput.CharacterControls.DetachLastRopePlacement.Enable();
        yield return new WaitUntil(() => hasDetachedLast);
        InteractionController.Instance.playerInput.CharacterControls.DetachFirstRopePlacement.Enable();
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to leave the room! Don't forget to use Q to remove red/oldest ropes, E to remove green/newset ropes, and R to confirm your point selection."));
        StartCoroutine(MoveThirdStageDoor());
        yield return new WaitUntil(() => hasExitedThirdStage);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitUntil(() => hasEnteredCrouchStage);
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can also push around the purple objects when you move against them, if they are tall enough, like the one in this room.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can crouch by holding LShift. Try crouching!"));
        InteractionController.Instance.playerInput.CharacterControls.Crouch.Enable();
        yield return new WaitUntil(() => hasCrouched);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Crouching allows you to stick to a purple object, while releasing LShift or jumping will unstick yourself.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You can sway around with the object you are crouching on when the object is hanging on ropes.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to leave the room."));
        yield return new WaitUntil(() => hasExitedCrouchStage);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Hints are now displayed. Try to reach the exit!"));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        
        InstructionGroupController.Instance.IsShown = true;
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.Instance.CurrentState;
        
        yield return new WaitUntil(() => hasCompleted);
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("You've completed the tutorial! Press Enter to exit the tutorial stage."));

    }
}
