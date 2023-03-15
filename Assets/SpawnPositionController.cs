using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionController : MonoBehaviour
{
    public Color inactiveColor;
    public Color activeColor;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

    }
}
