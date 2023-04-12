using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerDataProcessor : MonoBehaviour
{
    public struct Point
    {
        public int frameNumber;
        public float timestamp;
        public Vector3 worldPosition;
        public float speed;
        public string verticalState;
        public string horizontalState;
        public string stickTo;

        public Point(int frameNumber, float timestamp, Vector3 worldPosition, float speed, string verticalState, string horizontalState, string stickTo)
        {
            this.frameNumber = frameNumber;
            this.timestamp = timestamp;
            this.worldPosition = worldPosition;
            this.speed = speed;
            this.verticalState = verticalState;
            this.horizontalState = horizontalState;
            this.stickTo = stickTo;
        }
    }
    
    private readonly List<Point> _fullPoints = new();
    public List<Point> drawnPoints = new();

    public GameObject ropeRendererPrefab;
    public LineRenderer lineRenderer;
    public bool showLines = true;
    
    [HideInInspector] public int visualizationMode;

    [HideInInspector] public int lowerLimitFrame;
    [HideInInspector] public int upperLimitFrame;
    
    [HideInInspector] public int lowerLimitTimestamp;
    [HideInInspector] public int upperLimitTimestamp;

    private void OnValidate()
    {
        lineRenderer.enabled = showLines;
    }

    private void OnEnable()
    {
        DataVisualizer.OnDataUpdated += OnDataUpdated;
    }

    private void OnDisable()
    {
        DataVisualizer.OnDataUpdated -= OnDataUpdated;
    }
    
    private void OnDataUpdated()
    {
        if (_fullPoints.Count == 0) return;

        lineRenderer.positionCount = 0;
        drawnPoints.Clear();

        List<Point> timePoints = new List<Point>();
        List<Point> spatialPoints = new List<Point>();
        
        switch (visualizationMode)
        {
            case 0:
                timePoints = UpdateLinesByFrame();
                break;
            
            case 1:
                timePoints = UpdateLinesByTimestamp();
                break;
        }
        
        if (DataVisualizer.Instance.useBoundingBox)
        {
            spatialPoints = UpdateLinesByBoundingBox();
            drawnPoints = timePoints.Intersect(spatialPoints).ToList();
        }
        else drawnPoints = new List<Point>(timePoints);
        
        
        DrawLines();
    }

    private List<Point> UpdateLinesByTimestamp()
    {
        return _fullPoints.Where(point => point.timestamp > lowerLimitTimestamp && point.timestamp < upperLimitTimestamp).ToList();
    }

    private List<Point> UpdateLinesByFrame()
    {
        return _fullPoints.Where(point => point.frameNumber >= lowerLimitFrame && point.frameNumber <= upperLimitFrame).ToList();
    }

    private List<Point> UpdateLinesByBoundingBox()
    {
        Vector3 minBB = DataVisualizer.Instance.minBoundingBox;
        Vector3 maxBB = DataVisualizer.Instance.maxBoundingBox;

        return _fullPoints.Where(point => point.worldPosition.x > minBB.x &&
                                          point.worldPosition.y > minBB.y &&
                                          point.worldPosition.z > minBB.z &&
                                          point.worldPosition.x < maxBB.x &&
                                          point.worldPosition.y < maxBB.y &&
                                          point.worldPosition.z < maxBB.z
                                          ).ToList();
    }

    private void DrawLines()
    {
        lineRenderer.positionCount = drawnPoints.Count;
        
        int index = 0;
        foreach (var point in drawnPoints)
        {
            lineRenderer.SetPosition(index, point.worldPosition);
            index++;
        }
    }

    public void LoadData(string path, string[] ropePaths)
    {
        _fullPoints.Clear();

        string[] data = File.ReadAllLines(path);

        for (int i = 1; i < data.Length - 1; i++)
        {
            var line = data[i];
            
            var content = line.Split(";");
            float x = float.Parse(content[2].Replace(".", ","));
            float y = float.Parse(content[3].Replace(".", ","));
            float z = float.Parse(content[4].Replace(".", ","));

            Point point = new Point(
                int.Parse(content[0]),
                float.Parse(content[1].Replace(".", ",")),
                new Vector3(x, y, z),
                float.Parse(content[5].Replace(".", ",")),
                content[6],
                content[7],
                content[8]
            );
            
            _fullPoints.Add(point);
        }

        drawnPoints = new List<Point>(_fullPoints);
    }
}
