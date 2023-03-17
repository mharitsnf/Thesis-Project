using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLevelCore : LevelCore
{
    private void OnEnable()
    {
        InteractionController.Instance.playerInput.Enable();
        InteractionController.Instance.cameraInput.Enable();
    }
}
