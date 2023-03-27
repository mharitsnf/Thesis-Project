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
        InteractionController.Instance.playerInput.Enable();
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        InteractionController.Instance.cameraInput.Enable();
    }
}
