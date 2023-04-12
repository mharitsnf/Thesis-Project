using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MechanicsDataProcessor : MonoBehaviour
{
    struct MechanicsData
    {
        public int frameNumber;
        public float timestamp;
        public Vector3 worldPosition;
        public Quaternion rotation;
        public float speed;
        public bool hasSpringJoint;

        public MechanicsData(int frameNumber, float timestamp, Vector3 worldPosition, Quaternion rotation, float speed, bool hasSpringJoint)
        {
            this.frameNumber = frameNumber;
            this.timestamp = timestamp;
            this.worldPosition = worldPosition;
            this.rotation = rotation;
            this.speed = speed;
            this.hasSpringJoint = hasSpringJoint;
        }
    }
    
    private readonly List<MechanicsData> _fullPoints = new();
    private List<MechanicsData> _drawnPoints = new();
    private PlayerDataProcessor _pdp;

    public LineRenderer lineRenderer;
    public Color color;
    
    // Start is called before the first frame update
    void Start()
    {
        _pdp = transform.parent.parent.GetComponent<PlayerDataProcessor>();
        gameObject.SetActive(false);
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
        _drawnPoints.Clear();
        
        List<MechanicsData> timePoints = new List<MechanicsData>();
        List<MechanicsData> spatialPoints = new List<MechanicsData>();
        
        switch (_pdp.visualizationMode)
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
            _drawnPoints = timePoints.Intersect(spatialPoints).ToList();
        }
        else _drawnPoints = new List<MechanicsData>(timePoints);
        
        DrawLines();
    }
    
    private List<MechanicsData> UpdateLinesByTimestamp()
    {
        return _fullPoints.Where(point => point.timestamp > _pdp.lowerLimitTimestamp && point.timestamp < _pdp.upperLimitTimestamp).ToList();
    }

    private List<MechanicsData> UpdateLinesByFrame()
    {
        return _fullPoints.Where(point => point.frameNumber >= _pdp.lowerLimitFrame && point.frameNumber <= _pdp.upperLimitFrame).ToList();
    }
    
    private List<MechanicsData> UpdateLinesByBoundingBox()
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
        // lineRenderer.positionCount = _drawnPoints.Count;
        //
        // int index = 0;
        // foreach (var point in _drawnPoints)
        // {
        //     lineRenderer.SetPosition(index, point.worldPosition);
        //     index++;
        // }

        MechanicsData finalEntry = _drawnPoints[^1];
        transform.position = finalEntry.worldPosition;
        transform.rotation = finalEntry.rotation;
    }
    
    public void LoadData(string path)
    {
        _fullPoints.Clear();

        string[] data = File.ReadAllLines(path);

        for (int i = 1; i < data.Length - 1; i++)
        {
            var content = data[i].Split(";");
            float posX = float.Parse(content[2].Replace(".", ","));
            float posY = float.Parse(content[3].Replace(".", ","));
            float posZ = float.Parse(content[4].Replace(".", ","));
            float rotX = float.Parse(content[5].Replace(".", ",")) * Mathf.Rad2Deg;
            float rotY = float.Parse(content[6].Replace(".", ",")) * Mathf.Rad2Deg;
            float rotZ = float.Parse(content[7].Replace(".", ",")) * Mathf.Rad2Deg;

            MechanicsData ropeData = new MechanicsData(
                int.Parse(content[0]),
                float.Parse(content[1].Replace(".", ",")),
                new Vector3(posX, posY, posZ),
                Quaternion.Euler(rotX, rotY, rotZ),
                float.Parse(content[8].Replace(".", ",")),
                bool.Parse(content[9])
            );
            
            _fullPoints.Add(ropeData);
        }
    }
}
