using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public float gravityMultiplier;
    
    private void Awake()
    {
        SetGravity();
        SetCursor();
    }

    private void SetGravity()
    {
        Physics.gravity *= gravityMultiplier;
    }

    private void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
