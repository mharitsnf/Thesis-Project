using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RopeDataLogger : MonoBehaviour
{
    private readonly List<string> _data = new();

    private Rope _rope;

    private void Awake()
    {
        _rope = GetComponent<Rope>();
    }
    
    private void OnEnable()
    {
        LoggerController.OnWriteRow += AddRopeData;
    }

    private void OnDisable()
    {
        LoggerController.OnWriteRow -= AddRopeData;
    }

    private void AddRopeData()
    {
        if (_data.Count == 0) _data.Add("frame;timestamp;firstRB;x1;y1;z1;secondRB;x2;y2;z2");
        
        string row = "";
        row += LoggerController.Instance.frameCount + ";";
        row += Time.unscaledTime + ";";
        row += (_rope.firstEnd.rigidbody ? _rope.firstEnd.rigidbody.name : "none") + ";";
        row += Mathf.Round(_rope.firstEnd.worldPosition.x * 100f) * .01f + ";";
        row += Mathf.Round(_rope.firstEnd.worldPosition.y * 100f) * .01f + ";";
        row += Mathf.Round(_rope.firstEnd.worldPosition.z * 100f) * .01f + ";";
        row += (_rope.secondEnd.rigidbody ? _rope.secondEnd.rigidbody.name : "none") + ";";
        row += Mathf.Round(_rope.secondEnd.worldPosition.x * 100f) * .01f + ";";
        row += Mathf.Round(_rope.secondEnd.worldPosition.y * 100f) * .01f + ";";
        row += Mathf.Round(_rope.secondEnd.worldPosition.z * 100f) * .01f;
        
        _data.Add(row);
    }

    private void WriteFile()
    {
        string fullPath = $"{LoggerController.Instance.ropeFolderName}/{gameObject.name}.csv";
        
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
