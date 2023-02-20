using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform orientation;

    private void Update()
    {
        Vector3 viewDir = player.position - transform.position;
        viewDir.y = 0;
        orientation.forward = viewDir.normalized;
    }
}
