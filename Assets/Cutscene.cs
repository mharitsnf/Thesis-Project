using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public CinemachineVirtualCameraBase virtualCamera;
    public int playLimit = 1;
    
    public void StartCutscene()
    {
        if (PlayerData.Instance.isAiming) InteractionController.Instance.ForceQuitAiming();
        StartCoroutine(StartCutsceneHelper());
    }

    IEnumerator StartCutsceneHelper()
    {
        InteractionController.Instance.playerInput.Disable();

        CinemachineVirtualCameraBase previousCamera = PlayerData.Instance.cameraController.currentCamera;
        PlayerData.Instance.cameraController.SwitchVirtualCamera(virtualCamera);

        yield return new WaitForSecondsRealtime(1f);
        
        foreach (Transform child in transform)
        {
            DynamicObject dynamicObject = child.GetComponent<DynamicObject>();
            if (!dynamicObject) continue;
            yield return StartCoroutine(dynamicObject.MovePosition());
        }
        
        yield return new WaitForSecondsRealtime(1f);

        PlayerData.Instance.cameraController.SwitchVirtualCamera(previousCamera);
        
        InteractionController.Instance.playerInput.Enable();
        InteractionController.Instance.playerInput.CharacterControls.ExitAim.Disable();

        playLimit--;
    }
}
