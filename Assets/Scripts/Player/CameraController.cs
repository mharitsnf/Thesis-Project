using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public CinemachineVirtualCameraBase currentCamera;

    private void Start()
    {
        InitialSetup();
    }

    private void Update()
    {
        UpdateOrientation();
    }

    private void FixedUpdate()
    {
        RotateCinemachineFollow();
    }

    private void InitialSetup()
    {
        currentCamera = PlayerData.Instance.virtualCameras[0];
        
        Quaternion cinemachineFollowQuaternion = PlayerData.Instance.cinemachineFollow.transform.rotation;
        PlayerData.Instance.cinemachineFollowYaw = cinemachineFollowQuaternion.eulerAngles.y;
        PlayerData.Instance.cinemachineFollowPitch = cinemachineFollowQuaternion.eulerAngles.x;
    }

    private void UpdateOrientation()
    {
        Vector3 viewDir = gameObject.transform.position - PlayerData.Instance.realCamera.position;
        viewDir.y = 0;
        PlayerData.Instance.orientation.forward = viewDir.normalized;
    }

    private void RotateCinemachineFollow()
    {
        Vector2 cameraDelta = PlayerData.Instance.cameraLookDelta;
        float cameraSpeed = PlayerData.Instance.isAiming
            ? PlayerData.Instance.aimCameraSpeed
            : PlayerData.Instance.thirdPersonCameraSpeed;
        PlayerData.Instance.cinemachineFollowYaw += cameraDelta.x * cameraSpeed * Time.deltaTime * (PlayerData.Instance.invertX ? -1 : 1);
        PlayerData.Instance.cinemachineFollowPitch += cameraDelta.y * cameraSpeed * Time.deltaTime * (PlayerData.Instance.invertY ? -1 : 1);

        PlayerData.Instance.cinemachineFollowYaw = ClampAngle(PlayerData.Instance.cinemachineFollowYaw, float.MinValue, float.MaxValue);
        PlayerData.Instance.cinemachineFollowPitch = ClampAngle(PlayerData.Instance.cinemachineFollowPitch, PlayerData.Instance.minClamp, PlayerData.Instance.maxClamp);

        PlayerData.Instance.cinemachineFollow.transform.rotation = Quaternion.Euler(PlayerData.Instance.cinemachineFollowPitch, PlayerData.Instance.cinemachineFollowYaw, 0f);
    }
    
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SwitchVirtualCamera(CinemachineVirtualCameraBase newVirtualCamera)
    {
        newVirtualCamera.gameObject.SetActive(true);
    
        foreach (CinemachineVirtualCameraBase virtualCamera in PlayerData.Instance.virtualCameras)
        {
            if (virtualCamera != newVirtualCamera) virtualCamera.gameObject.SetActive(false);
        }

        currentCamera = newVirtualCamera;
    }
}
