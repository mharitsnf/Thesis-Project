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
                TutorialCore.Instance.hasExitedThirdStage = true;
                break;
            case "Crouch Stage Entrance":
                PlayerData.Instance.initialPosition = PlayerData.Instance.transform.position;
                TutorialCore.Instance.hasEnteredCrouchStage = true;
                break;
            case "Crouch Stage Exit":
                PlayerData.Instance.initialPosition = PlayerData.Instance.transform.position;
                TutorialCore.Instance.hasExitedCrouchStage = true;
                break;
            case "Final Exit":
                PlayerData.Instance.initialPosition = PlayerData.Instance.transform.position;
                TutorialCore.Instance.hasCompleted = true;
                break;
        }
    }
}
