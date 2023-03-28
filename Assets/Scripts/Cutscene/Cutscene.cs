using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public CinemachineVirtualCameraBase virtualCamera;
    public int playLimit = 1;
    public int switchCameraLimit = 1;
    public bool isLastTriggerPlayed;
    public bool isMovingTogether;
    public bool isCoroutineRunning;

    public bool atTargetPosition;
    
    public void StartCutscene()
    {
        if (PlayerData.Instance.isAiming) InteractionController.Instance.ForceQuitAiming();
        StartCoroutine(StartCutsceneHelper());
    }

    IEnumerator StartCutsceneHelper()
    {
        CinemachineVirtualCameraBase previousCamera = PlayerData.Instance.cameraController.currentCamera;
        
        if (switchCameraLimit > 0 || switchCameraLimit == -1)
        {
            InteractionController.Instance.playerInput.Disable();
            PlayerData.Instance.cameraController.SwitchVirtualCamera(virtualCamera);
            yield return new WaitForSecondsRealtime(1f);
        }

        if ((playLimit > 0 || playLimit == -1) && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            // Move the objects
            foreach (Transform child in transform)
            {
                DynamicObject dynamicObject = child.GetComponent<DynamicObject>();
                if (!dynamicObject) continue;

                
                if (!isMovingTogether) yield return StartCoroutine(dynamicObject.MovePosition(!atTargetPosition));
                else StartCoroutine(dynamicObject.MovePosition(!atTargetPosition));
            }

            atTargetPosition = !atTargetPosition;
            isCoroutineRunning = false;
        }
        
        if (switchCameraLimit > 0 || switchCameraLimit == -1)
        {
            yield return new WaitForSecondsRealtime(1f);
            PlayerData.Instance.cameraController.SwitchVirtualCamera(previousCamera);
            InteractionController.Instance.playerInput.Enable();
            InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();
        }

        if (playLimit > 0) playLimit--;
        if (switchCameraLimit > 0) switchCameraLimit--;
    }
}
