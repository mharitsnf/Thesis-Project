using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public float gravityMultiplier;
    public Vector3 initialGravity = new Vector3(0, -9.81000042f, 0);
    private int _targetFrameRate = 60;

    private void Awake()
    {
        SetFrameRate();
        SetGravity();
        SetCursor();
    }

    private void SetFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFrameRate;
    }

    private void SetGravity()
    {
        Physics.gravity = initialGravity * gravityMultiplier;
    }

    private void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
