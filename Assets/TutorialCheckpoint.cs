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
                TutorialCore.Instance.hasExitFirstStage = true;
                break;
            case "Third Stage Entrance":
                TutorialCore.Instance.hasEnteredThirdStage = true;
                break;
            case "Third Stage Exit":
                TutorialCore.Instance.hasExitThirdStage = true;
                break;
            case  "Last Stage Entrance":
                TutorialCore.Instance.hasEnteredLastStage = true;
                break;
        }
    }
}
