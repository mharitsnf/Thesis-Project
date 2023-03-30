using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCore : MonoBehaviour
{
    protected bool _panelDone;
    
    private void OnEnable()
    {
        InteractionController.OnNextPanel += OnNextPanel;
    }

    private void OnDisable()
    {
        InteractionController.OnNextPanel -= OnNextPanel;
    }

    private void OnNextPanel()
    {
        _panelDone = true;
    }
}
