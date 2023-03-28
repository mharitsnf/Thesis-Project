using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeDataLogger : MonoBehaviour
{
    public static RopeDataLogger Instance { get; private set; }
    
    private readonly List<String> _data = new();
    private String _fileName;
    private String _folderName = "RopeData";

    public int frameInterval = 4;
    private int _frameCounter;

    private Transform _playerTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _folderName += "/" + SceneManager.GetActiveScene().name;
        String pathToFolder = Application.dataPath + "/" + _folderName;

        if (!Directory.Exists(pathToFolder))
        {
            Directory.CreateDirectory(pathToFolder);
        }

        _fileName = SceneManager.GetActiveScene().name;
        _fileName = pathToFolder + "/" + _fileName + ".csv";
        Debug.Log(_fileName);
        
        _data.Add("frame;timestamp;x;y;z;speed;hState;vState;isOnSlope;crouchingOn");
    }

    private void AddPlayerData()
    {
        String row = "";
        row += _frameCounter + ";";
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
        if (_frameCounter % frameInterval == 0)
        {
            AddPlayerData();
        }

        _frameCounter++;
    }

    private void WriteFile()
    {
        
        TextWriter tw = new StreamWriter(_fileName, false);

        foreach (var row in _data)
        {
            tw.WriteLine(row);
        }
        
        tw.Close();

        print("File written!");
    }

    private void OnDestroy()
    {
        WriteFile();
    }
}
