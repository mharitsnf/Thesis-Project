using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public class EndData
    {
        public readonly Rigidbody rigidbody;
        public readonly Vector3 localPosition;
        public Vector3 worldPosition;

        public EndData(Rigidbody rigidbody, Vector3 localPosition, Vector3 worldPosition)
        {
            this.rigidbody = rigidbody;
            this.localPosition = localPosition;
            this.worldPosition = worldPosition;
        }
        
        public EndData(Vector3 worldPosition)
        {
            this.worldPosition = worldPosition;
        }
    }

    private EndData _firstEnd;
    private EndData _secondEnd;
    public SpringJoint joint;
    private LineRenderer _lineRenderer;
    private bool _canDraw;

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
        if (_firstEnd != null && _firstEnd.rigidbody) _firstEnd.worldPosition = _firstEnd.rigidbody.transform.TransformPoint(_firstEnd.localPosition);
        if (_secondEnd != null && _secondEnd.rigidbody) _secondEnd.worldPosition = _secondEnd.rigidbody.transform.TransformPoint(_secondEnd.localPosition);
    }

    private void DrawLine()
    {
        if (!_canDraw) return;
        
        if (_firstEnd != null)
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, _firstEnd.rigidbody ? _firstEnd.rigidbody.transform.position : _firstEnd.worldPosition);
        }

        if (_secondEnd != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(1, _secondEnd.rigidbody ? _secondEnd.rigidbody.transform.position : _secondEnd.worldPosition);
        }
    }
    
    public bool PlaceEnd(RaycastHit hit)
    {
        if (_firstEnd != null && !_firstEnd.rigidbody && !hit.rigidbody) return false;
        if (_firstEnd != null && _firstEnd.rigidbody == hit.rigidbody) return false;
        
        EndData end = !hit.rigidbody ? new EndData(hit.point) : new EndData(hit.rigidbody, hit.rigidbody.transform.InverseTransformPoint(hit.point), hit.point);

        if (_firstEnd == null) _firstEnd = end;
        else _secondEnd = end;
        
        return true;
    }

    public void CreateJoint()
    {
        if (_firstEnd.rigidbody && _secondEnd.rigidbody)
        {
            _firstEnd.rigidbody.freezeRotation = true;
            _secondEnd.rigidbody.freezeRotation = true;
            joint = SetupJoint(_firstEnd.rigidbody, (_firstEnd.worldPosition - _secondEnd.worldPosition).magnitude);
        
            joint.anchor = _firstEnd.localPosition;
            joint.connectedAnchor = _secondEnd.localPosition;
        }
        else if (_firstEnd.rigidbody && !_secondEnd.rigidbody)
        {
            _firstEnd.rigidbody.freezeRotation = true;
            joint = SetupJoint(_firstEnd.rigidbody, (_firstEnd.worldPosition - _secondEnd.worldPosition).magnitude);
        
            joint.connectedAnchor = _secondEnd.worldPosition;
        }
        // else if (!_firstEnd.rigidbody && _secondEnd.rigidbody)
        // {
        //     _secondEnd.rigidbody.freezeRotation = true;
        //     joint = SetupJoint(_secondEnd.rigidbody, (_firstEnd.worldPosition - _secondEnd.worldPosition).magnitude);
        //
        //     joint.anchor = _secondEnd.localPosition;
        //     joint.connectedAnchor = _firstEnd.worldPosition;
        // }

        _canDraw = true;
    }

    private SpringJoint SetupJoint(Rigidbody rb, float distance)
    {
        SpringJoint j = rb.gameObject.AddComponent<SpringJoint>();
        j.autoConfigureConnectedAnchor = false;
        j.enableCollision = true;

        j.maxDistance = distance * PlayerData.Instance.maxDistanceMultiplier;
        j.minDistance = distance * PlayerData.Instance.minDistanceMultiplier;
        
        j.spring = rb.mass * PlayerData.Instance.springinessMultiplier;
        j.damper = PlayerData.Instance.springDamper;
        j.massScale = PlayerData.Instance.springMassScale;

        return j;
    }
}