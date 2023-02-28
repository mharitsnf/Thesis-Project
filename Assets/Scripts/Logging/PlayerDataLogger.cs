using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataLogger : MonoBehaviour
{
    public String filename;
    public int frameInterval = 4;

    private readonly List<String> _data = new();
    private int _frameCount = 5;

    private Transform _playerTransform;

    private void Awake()
    {
        if (filename == "") filename = "PlayerData";
        
        filename = Application.dataPath + "/" + filename + ".csv";
        Debug.Log(filename);
        
        _data.Add("timestamp;x;y;z;magnitude;hState;vState;isOnSlope;attachedJoint");
    }

    private void AddPlayerData()
    {
        String row = "";
        row += Time.unscaledTime + ";";
        Vector3 position = PlayerData.Instance.transform.position;
        row += position.x + ";";
        row += position.y + ";";
        row += position.z + ";";
        row += Mathf.Round(PlayerData.Instance.rigidBody.velocity.magnitude * 10f) * .1f + ";";
        row += PlayerData.Instance.horizontalStateController.currentState + ";";
        row += PlayerData.Instance.verticalStateController.currentState + ";";
        row += PlayerData.Instance.isOnSlope + ";";
        row += !PlayerData.Instance.fixedJoint ? "none" : PlayerData.Instance.fixedJoint.connectedBody.gameObject.name;
        
        _data.Add(row);
    }

    private void Update()
    {
        if (_frameCount > frameInterval)
        {
            AddPlayerData();
            _frameCount = 0;
        }

        _frameCount++;
    }

    private void WriteFile()
    {
        TextWriter tw = new StreamWriter(filename, false);

        foreach (var row in _data)
        {
            tw.WriteLine(row);
        }
        
        tw.Close();

        Debug.Log("File written!");
    }

    private void OnApplicationQuit()
    {
        WriteFile();
    }
}
