using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RopeDataProcessor : MonoBehaviour
{
    struct RopeData
    {
        public int frameNumber;
        public float timestamp;
        public string firstRb;
        public Vector3 firstEnd;
        public string secondRb;
        public Vector3 secondEnd;

        public RopeData(int frameNumber, float timestamp, string firstRb, Vector3 firstEnd, string secondRb, Vector3 secondEnd)
        {
            this.frameNumber = frameNumber;
            this.timestamp = timestamp;
            this.firstRb = firstRb;
            this.firstEnd = firstEnd;
            this.secondRb = secondRb;
            this.secondEnd = secondEnd;
        }
    }

    private List<RopeData> _fullPoints = new();

    public LineRenderer lineRenderer;
    public Color color;
    
    
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
        
    }

    public void LoadData(string path)
    {
        _fullPoints.Clear();

        string[] data = File.ReadAllLines(path);

        for (int i = 1; i < data.Length - 1; i++)
        {
            var content = data[i].Split(";");
            float x1 = float.Parse(content[3].Replace(".", ","));
            float y1 = float.Parse(content[4].Replace(".", ","));
            float z1 = float.Parse(content[5].Replace(".", ","));
            float x2 = float.Parse(content[7].Replace(".", ","));
            float y2 = float.Parse(content[8].Replace(".", ","));
            float z2 = float.Parse(content[9].Replace(".", ","));

            RopeData ropeData = new RopeData(
                int.Parse(content[0]),
                float.Parse(content[1].Replace(".", ",")),
                content[2],
                new Vector3(x1, y1, z1),
                content[6],
                new Vector3(x2, y2, z2)
            );
            _fullPoints.Add(ropeData);
        }
    }
}
