using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicObject : MonoBehaviour
{
    public Animator animator;

    public void StartAnimate(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
