using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [ReadOnly] public List<Vector3> ends = new();
    private readonly Dictionary<int, Rigidbody> _dynamicEnds = new();
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateDynamicIndexes();
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    private void UpdateDynamicIndexes()
    {
        foreach (KeyValuePair<int, Rigidbody> entry in _dynamicEnds)
        {
            ends[entry.Key] = entry.Value.position;
        }
    }

    private void DrawLine()
    {
        _lineRenderer.positionCount = ends.Count;
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, ends[i]);
        }
    }
    

    public void PlaceEnd()
    {
        if (Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward, out var hit, PlayerData.Instance.rayCastDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player")) return;
            
            ends.Add(hit.point);

            if (hit.rigidbody) _dynamicEnds.Add(ends.Count - 1, hit.rigidbody);
        }
    }
}