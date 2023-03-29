using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoggerController : MonoBehaviour
{
    public delegate void WriteRow();
    public static event WriteRow OnWriteRow;
    public int writeFrameInterval = 5;
    public int frameCount;
    
    public static LoggerController Instance { get; private set; }

    public string baseFolderName = "GameplayData";
    public string playerFolderName = "Player";
    public string ropeFolderName = "Ropes";
    public string mechanicFolderName = "Mechanics";

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        baseFolderName = $"{Application.dataPath}/{baseFolderName}/{SceneManager.GetActiveScene().name}";
        playerFolderName = $"{baseFolderName}/{playerFolderName}";
        ropeFolderName = $"{baseFolderName}/{ropeFolderName}";
        mechanicFolderName = $"{baseFolderName}/{mechanicFolderName}";
        
        if (!Directory.Exists(baseFolderName))
        {
            Directory.CreateDirectory(baseFolderName);
        }
        
        DirectoryInfo di = new DirectoryInfo(baseFolderName);
        foreach (FileInfo file in di.EnumerateFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.EnumerateDirectories())
        {
            dir.Delete(true); 
        }
        
        if (!Directory.Exists(playerFolderName))
        {
            Directory.CreateDirectory(playerFolderName);
        }
        
        if (!Directory.Exists(ropeFolderName))
        {
            Directory.CreateDirectory(ropeFolderName);
        }
        
        if (!Directory.Exists(mechanicFolderName))
        {
            Directory.CreateDirectory(mechanicFolderName);
        }
        
    }

    private void LateUpdate()
    {
        if (frameCount % writeFrameInterval == 0)
        {
            if (OnWriteRow != null) OnWriteRow();
        }
        frameCount++;
    }
}
