using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MechanicsDataLogger : MonoBehaviour
{
    private readonly List<string> _data = new();

    private Transform _playerTransform;
    private Transform _parent;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _parent = transform.parent;
        _rigidbody = _parent.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        LoggerController.OnWriteRow += AddMechanicsData;
    }

    private void OnDisable()
    {
        LoggerController.OnWriteRow -= AddMechanicsData;
    }

    private void AddMechanicsData()
    {
        if (_data.Count == 0) _data.Add("frame;timestamp;x;y;z;rotX;rotY;rotZ;speed;hasSpringJoint");
        
        string row = "";
        row += LoggerController.Instance.frameCount + ";";
        row += Time.unscaledTime + ";";
        Vector3 position = _parent.position;
        row += Mathf.Round(position.x * 100f) * .01f + ";";
        row += Mathf.Round(position.y * 100f) * .01f + ";";
        row += Mathf.Round(position.z * 100f) * .01f + ";";
        Quaternion rotation = _parent.rotation;
        row += Mathf.Round(rotation.x * 100f) * .01f + ";";
        row += Mathf.Round(rotation.y * 100f) * .01f + ";";
        row += Mathf.Round(rotation.z * 100f) * .01f + ";";
        row += Mathf.Round(_rigidbody.velocity.magnitude * 100f) * .01f + ";";
        row += (bool) _parent.GetComponent<SpringJoint>();
        
        _data.Add(row);
    }

    private void WriteFile()
    {
        string fullPath = $"{LoggerController.Instance.mechanicFolderName}/{_parent.gameObject.name}.csv";

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
