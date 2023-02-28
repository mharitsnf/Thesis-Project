using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugLogSaver : MonoBehaviour
{
    public string filename = "";
    void OnEnable() { Application.logMessageReceived += Log;  }
    void OnDisable() { Application.logMessageReceived -= Log; }

    private void Log(string logString, string stackTrace, LogType type)
    {
        if (filename == "")
        {
            string d = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Desktop) + "/YOUR_LOGS";
            System.IO.Directory.CreateDirectory(d);
            filename = d + "/my_happy_log.txt";
        }
     
        try {
            System.IO.File.AppendAllText(filename, logString + "\n");
        }
        catch
        {
            // ignored
        }
    }
}