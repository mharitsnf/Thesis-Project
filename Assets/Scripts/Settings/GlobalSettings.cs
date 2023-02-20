using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public float gravityMultiplier;
    
    private void Awake()
    {
        Physics.gravity *= gravityMultiplier;
    }
}
