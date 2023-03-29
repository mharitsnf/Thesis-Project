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
        yield return new WaitForSeconds(1);
        InstructionGroupController.Instance.IsShown = true;
        InstructionGroupController.Instance.CurrentState = InstructionGroupController.DisplayState.NotAiming;
        
        InteractionController.Instance.playerInput.Disable();
        InteractionController.Instance.cameraInput.Disable();
        
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Collect all 6 orbs spread across the map!"));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(PanelController.Instance.HidePanel());
        yield return StartCoroutine(PanelController.Instance.ShowPanel("Try to be as fast as you can."));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(PanelController.Instance.HidePanel());

        InteractionController.Instance.playerInput.Enable();
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        InteractionController.Instance.cameraInput.Enable();
    }
}
