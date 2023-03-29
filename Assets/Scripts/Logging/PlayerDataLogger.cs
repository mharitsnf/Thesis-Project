using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataLogger : MonoBehaviour
{
    public static PlayerDataLogger Instance { get; private set; }
    
    private readonly List<string> _data = new();

    private Transform _playerTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void OnEnable()
    {
        LoggerController.OnWriteRow += AddPlayerData;
    }

    private void OnDisable()
    {
        LoggerController.OnWriteRow -= AddPlayerData;
    }

    private void AddPlayerData()
    {
        if (_data.Count == 0) _data.Add("frame;timestamp;x;y;z;speed;hState;vState;isOnSlope;crouchingOn");

        string row = "";
        row += LoggerController.Instance.frameCount + ";";
        row += Time.unscaledTime + ";";
        Vector3 position = PlayerData.Instance.transform.position;
        row += Mathf.Round(position.x * 100f) * .01f + ";";
        row += Mathf.Round(position.y * 100f) * .01f + ";";
        row += Mathf.Round(position.z * 100f) * .01f + ";";
        row += Mathf.Round(PlayerData.Instance.rigidBody.velocity.magnitude * 100f) * .01f + ";";
        row += PlayerData.Instance.horizontalStateController.currentState + ";";
        row += PlayerData.Instance.verticalStateController.currentState + ";";
        row += PlayerData.Instance.isOnSlope + ";";
        row += !PlayerData.Instance.fixedJoint ? "none" : PlayerData.Instance.fixedJoint.connectedBody.gameObject.name;
        
        _data.Add(row);
    }

    private void WriteFile()
    {
        string fullPath = $"{LoggerController.Instance.playerFolderName}/data.csv";
        TextWriter tw = new StreamWriter(fullPath, false);

        foreach (var row in _data)
        {
            tw.WriteLine(row);
        }
        
        tw.Close();

        print($"{fullPath} written!");
    }

    private void OnDestroy()
    {
        WriteFile();
    }
}
