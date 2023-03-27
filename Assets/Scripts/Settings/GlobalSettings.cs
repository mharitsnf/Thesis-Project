using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public float gravityMultiplier;
    public Vector3 initialGravity = new Vector3(0, -9.81000042f, 0);

    private void Awake()
    {
        SetGravity();
        SetCursor();
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
