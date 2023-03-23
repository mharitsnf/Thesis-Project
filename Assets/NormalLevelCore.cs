using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLevelCore : LevelCore
{

    private void Start()
    {
        InstructionGroupController.Instance.IsShown = true;
    }
}
