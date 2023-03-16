using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        switch (name)
        {
            case "First Stage Exit":
                TutorialInteractionController.Instance.hasExitFirstStage = true;
                break;
            case "Third Stage Entrance":
                TutorialInteractionController.Instance.hasEnteredThirdStage = true;
                break;
        }
    }
}
