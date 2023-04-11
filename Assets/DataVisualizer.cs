using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class DataVisualizer : MonoBehaviour
{
    public static DataVisualizer Instance { get; private set; }
    
    public delegate void DrawLines();
    public static event DrawLines OnDrawLines;

    public string folderRelativePath;
    public GameObject rendererPrefab;

    private string[] _directories;

    [HideInInspector] public int visualizationMode;
    
    [HideInInspector] public int lowerLimitPercentage;
    [HideInInspector] public int upperLimitPercentage;
    
    [HideInInspector] public int lowerLimitFrame;
    [HideInInspector] public int upperLimitFrame;
    
    [HideInInspector] public int lowerLimitTimestamp;
    [HideInInspector] public int upperLimitTimestamp;

    public bool useBoundingBox;
    [HideInInspector] public Vector3 minBoundingBox;
    [HideInInspector] public Vector3 maxBoundingBox;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _directories = Directory.GetDirectories($"{Application.dataPath}/{folderRelativePath}");

        foreach (var participant in _directories)
        {
            string playerFile = $"{participant}/CSGScene Original 2/Player/data.csv";
            // string[] ropeFiles = Directory.GetFiles($"{participant}/CSGScene Original 2/Ropes");

            GameObject dataRendererObject = Instantiate(rendererPrefab, transform);
            dataRendererObject.name = participant.Split("\\")[1];

            dataRendererObject.GetComponent<PlayerDataProcessor>().LoadData(playerFile);
            
        }
        
        InvokeDrawLines();
    }

    public void InvokeDrawLines()
    {
        OnDrawLines?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (useBoundingBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(minBoundingBox, 2);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(maxBoundingBox, 2);
            
            Gizmos.color = Color.black;
            Gizmos.DrawLine(minBoundingBox, new Vector3(maxBoundingBox.x, minBoundingBox.y, minBoundingBox.z));
            Gizmos.DrawLine(minBoundingBox, new Vector3(minBoundingBox.x, maxBoundingBox.y, minBoundingBox.z));
            Gizmos.DrawLine(new Vector3(minBoundingBox.x, maxBoundingBox.y, minBoundingBox.z), new Vector3(maxBoundingBox.x, maxBoundingBox.y, minBoundingBox.z));
            Gizmos.DrawLine(new Vector3(maxBoundingBox.x, minBoundingBox.y, minBoundingBox.z), new Vector3(maxBoundingBox.x, maxBoundingBox.y, minBoundingBox.z));
            
            Gizmos.DrawLine(new Vector3(maxBoundingBox.x, minBoundingBox.y, minBoundingBox.z), new Vector3(maxBoundingBox.x, minBoundingBox.y, maxBoundingBox.z));
            Gizmos.DrawLine(new Vector3(maxBoundingBox.x, maxBoundingBox.y, minBoundingBox.z), maxBoundingBox);
            Gizmos.DrawLine(new Vector3(minBoundingBox.x, maxBoundingBox.y, minBoundingBox.z), new Vector3(minBoundingBox.x, maxBoundingBox.y, maxBoundingBox.z));
            Gizmos.DrawLine(minBoundingBox, new Vector3(minBoundingBox.x, minBoundingBox.y, maxBoundingBox.z));
            
            Gizmos.DrawLine(new Vector3(minBoundingBox.x, minBoundingBox.y, maxBoundingBox.z), new Vector3(minBoundingBox.x, maxBoundingBox.y, maxBoundingBox.z));
            Gizmos.DrawLine(new Vector3(minBoundingBox.x, minBoundingBox.y, maxBoundingBox.z), new Vector3(maxBoundingBox.x, minBoundingBox.y, maxBoundingBox.z));
            Gizmos.DrawLine(new Vector3(minBoundingBox.x, maxBoundingBox.y, maxBoundingBox.z), maxBoundingBox);
            Gizmos.DrawLine(new Vector3(maxBoundingBox.x, minBoundingBox.y, maxBoundingBox.z), maxBoundingBox);
        }
    }
}
