using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerDataProcessor : MonoBehaviour
{
    struct Point
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
    
    private List<Point> _fullPoints = new();
    private List<Point> _drawnPoints = new();
    public Color color;

    public LineRenderer lineRenderer;
    public bool showLines = true;

    private void OnValidate()
    {
        lineRenderer.enabled = showLines;
    }

    void Start()
    {
        // Color setup
        Color firstColor = Random.ColorHSV();
        color = firstColor;
        float h, s, v;
        Color.RGBToHSV(firstColor, out h, out s, out v);
        firstColor = Color.HSVToRGB(h, 0, v);
        Color secondColor = Color.HSVToRGB(h, 1, v);
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] {new GradientColorKey(firstColor, 0f), new GradientColorKey(secondColor, 1f)},
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0f) }
            );
        
        lineRenderer.colorGradient = gradient;
    }

    private void OnEnable()
    {
        DataVisualizer.OnDrawLines += OnDrawLines;
    }

    private void OnDisable()
    {
        DataVisualizer.OnDrawLines -= OnDrawLines;
    }
    
    private void OnDrawLines()
    {
        if (_fullPoints.Count == 0) return;
        lineRenderer.positionCount = 0;
        _drawnPoints.Clear();

        List<Point> timePoints;
        List<Point> spatialPoints = new List<Point>();
        switch (DataVisualizer.Instance.visualizationMode)
        {
            case 0:
                timePoints = UpdateLinesByPercentage();
                if (DataVisualizer.Instance.useBoundingBox) spatialPoints = UpdateLinesByBoundingBox();
                _drawnPoints = timePoints.Union(spatialPoints).ToList();
                break;
                
            case 1:
                timePoints = UpdateLinesByFrame();
                if (DataVisualizer.Instance.useBoundingBox) spatialPoints = UpdateLinesByBoundingBox();
                _drawnPoints = timePoints.Union(spatialPoints).ToList();
                break;
            
            case 2:
                timePoints = UpdateLinesByTimestamp();
                if (DataVisualizer.Instance.useBoundingBox) spatialPoints = UpdateLinesByBoundingBox();
                _drawnPoints = timePoints.Union(spatialPoints).ToList();
                break;
        }
        
        
        DrawLines();
    }

    private List<Point> UpdateLinesByTimestamp()
    {
        return _fullPoints.Where(point => point.timestamp > DataVisualizer.Instance.lowerLimitTimestamp && point.timestamp < DataVisualizer.Instance.upperLimitTimestamp).ToList();
    }

    private List<Point> UpdateLinesByFrame()
    {
        return _fullPoints.Where(point => point.frameNumber >= DataVisualizer.Instance.lowerLimitFrame && point.frameNumber <= DataVisualizer.Instance.upperLimitFrame).ToList();
    }
    
    private List<Point> UpdateLinesByPercentage()
    {
        int rowsCount = _fullPoints.Count;
        int startRow = (int) Mathf.Floor(rowsCount * (DataVisualizer.Instance.lowerLimitPercentage / 100f));
        int endRow = (int) Mathf.Floor(rowsCount * (DataVisualizer.Instance.upperLimitPercentage / 100f));


        return _fullPoints.GetRange(startRow, endRow - startRow).ToList();
    }

    private List<Point> UpdateLinesByBoundingBox()
    {
        Vector3 minBB = DataVisualizer.Instance.minBoundingBox;
        Vector3 maxBB = DataVisualizer.Instance.maxBoundingBox;

        IEnumerable<Point> insideBB = _drawnPoints.Where(point => point.worldPosition.x > minBB.x &&
                                                                  point.worldPosition.y > minBB.y &&
                                                                  point.worldPosition.z > minBB.z &&
                                                                  point.worldPosition.x < maxBB.x &&
                                                                  point.worldPosition.y < maxBB.y &&
                                                                  point.worldPosition.z < maxBB.z);

        return insideBB.ToList();
    }

    private void DrawLines()
    {
        lineRenderer.positionCount = _drawnPoints.Count;
        
        int index = 0;
        foreach (var point in _drawnPoints)
        {
            lineRenderer.SetPosition(index, point.worldPosition);
            index++;
        }
    }

    public void LoadData(string path)
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

        _drawnPoints = new List<Point>(_fullPoints);
    }
}
