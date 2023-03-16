using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCore : LevelCore
{
    public static TutorialCore Instance { get; private set; }
    
    public bool hasMoved;
    private float _moveDistance;

    public bool hasLookedAround;
    private float _lookAroundAmount;

    public bool hasJumped;
    public int jumpCount;

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

    public bool hasExitThirdStage;

    public bool hasEnteredLastStage;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        InstructionGroupController.Instance.IsShown = false;
        yield return new WaitForSecondsRealtime(3f);

        // Teach movement and camera
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Use WASD to move around and Mouse to move the camera."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.Move.Enable();
        TutorialInteractionController.Instance.cameraInput.CameraLook.Rotate.Enable();
        yield return new WaitUntil(() => hasMoved && hasLookedAround);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(3f);

        TutorialInteractionController.Instance.playerInput.CharacterControls.Jump.Enable();
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Use Space to jump. Try jumping a few times."));
        yield return new WaitUntil(() => hasJumped);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try to get out of the room."));
        yield return new WaitUntil(() => hasExitFirstStage);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());

        yield return new WaitUntil(() => hasEnteredThirdStage);
        
        TutorialInteractionController.Instance.playerInput.CharacterControls.Move.Disable();
        TutorialInteractionController.Instance.cameraInput.CameraLook.Rotate.Disable();
        TutorialInteractionController.Instance.playerInput.CharacterControls.Jump.Enable();
        PlayerData.Instance.moveDirection = Vector2.zero;
        PlayerData.Instance.cameraLookDelta = Vector2.zero;
        PlayerData.Instance.cameraController.SwitchVirtualCamera(2);
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can create elastic ropes from these purple objects."));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can select objects in aim mode."));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());

        PlayerData.Instance.cameraController.SwitchVirtualCamera(0);
        yield return new WaitForSecondsRealtime(.5f);
        TutorialInteractionController.Instance.playerInput.CharacterControls.Move.Enable();
        TutorialInteractionController.Instance.cameraInput.CameraLook.Rotate.Enable();
        TutorialInteractionController.Instance.playerInput.CharacterControls.Jump.Enable();

        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Use Right Click to enter aim mode."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.EnterAim.Enable();
        yield return new WaitUntil(() => hasEnteredAimMode);
        TutorialInteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Time is slowed down when you are in aim mode."));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try to exit aim mode using Right Click or Q."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.ExitAim.Enable();
        yield return new WaitUntil(() => hasExitedAimMode);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Let's try selecting objects. Enter aim mode with Right Click, and then Left Click on the object to select it."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.SelectObject.Enable();
        yield return new WaitUntil(() => hasSelectedObject);
        TutorialInteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Great! Now select anything other than the object."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.SelectSurface.Enable();
        yield return new WaitUntil(() => hasSelectedSurface);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can have as many points as you like. The ropes will be connected from the object to these points."));
        yield return new WaitForSecondsRealtime(5f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try confirming your selection with E."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.ConfirmRopePlacement.Enable();
        yield return new WaitUntil(() => hasConfirmedSelection);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can also cancel the points by exiting aim mode."));
        yield return new WaitForSecondsRealtime(5f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try repeating the process two more times."));
        yield return new WaitUntil(() => ropeBatchCount > 2);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Ropes comes in different colors: red are the oldest, green are the newest, yellow are anything in between."));
        yield return new WaitForSecondsRealtime(7f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("You can destroy red or green ropes."));
        yield return new WaitForSecondsRealtime(5f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Oldest/red ropes can be destroyed using Left Shift, while green/newest ropes using Q."));
        yield return new WaitForSecondsRealtime(7f);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try destroying the red ropes using Left Shift."));
        TutorialInteractionController.Instance.playerInput.CharacterControls.DetachFirstRopePlacement.Enable();
        yield return new WaitUntil(() => hasDetachedFirst);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Now try destroying green ropes using Q"));
        TutorialInteractionController.Instance.playerInput.CharacterControls.DetachLastRopePlacement.Enable();
        yield return new WaitUntil(() => hasDetachedLast);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Try to leave the room!"));
        yield return new WaitUntil(() => hasExitThirdStage);
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());

        yield return new WaitUntil(() => hasEnteredLastStage);
        
        yield return StartCoroutine(TutorialPanelController.Instance.ShowPanel("Hints are now displayed on the bottom of the screen. Good luck!"));
        yield return new WaitForSecondsRealtime(3f);
        InstructionGroupController.Instance.IsShown = true;
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.Instance.CurrentState;
        yield return StartCoroutine(TutorialPanelController.Instance.HidePanel());
    }
}
