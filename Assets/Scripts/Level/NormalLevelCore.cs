using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLevelCore : LevelCore
{
    private void Start()
    {
        StartCoroutine(LevelSequence());
    }

    IEnumerator LevelSequence()
    {
        yield return new WaitForSecondsRealtime(1f);
        InstructionGroupController.Instance.IsShown = true;
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        
        InteractionController.Instance.playerInput.Disable();
        InteractionController.Instance.cameraInput.Disable();

        yield return StartCoroutine(PanelController.Instance.ShowPanel("Collect all 6 orbs spread across the map!", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return new WaitForSecondsRealtime(.5f);
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to be as fast as you can.", true));
        yield return new WaitUntil(() => _panelDone);
        _panelDone = false;
        yield return StartCoroutine(PanelController.Instance.HidePanel());

        InteractionController.Instance.playerInput.Enable();
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        InteractionController.Instance.cameraInput.Enable();
        
        InteractionController.Instance.playerInput.OtherInteraction.Disable();

        yield return new WaitUntil(() => PlayerData.Instance.objectiveCollectedAmount == 6);
        
        if (PlayerData.Instance.isAiming) InteractionController.Instance.ForceQuitAiming();
        InteractionController.Instance.playerInput.CharacterControls.Disable();
        InteractionController.Instance.cameraInput.Enable();
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Congratulations! You've completed the game! You can exit the game."));
        
        
        
    }
}
