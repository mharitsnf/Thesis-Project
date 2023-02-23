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

        public EndData(Rigidbody rigidbody, Vector3 localPosition)
        {
            this.rigidbody = rigidbody;
            this.localPosition = localPosition;
        }
        
        public EndData(Vector3 worldPosition)
        {
            this.worldPosition = worldPosition;
        }
    }

    [ReadOnly] public readonly List<EndData> ends = new();
    private LineRenderer _lineRenderer;
    private bool _foundHit;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateDynamicIndexes();
    }

    private void FixedUpdate()
    {
        CheckPassingRigidbody();
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    private void CheckPassingRigidbody()
    {
        if (ends.Count < 2) return;
        if (ends[0].rigidbody || ends[1].rigidbody) return;
        if (_foundHit) return;

        bool rayCast1 = Physics.Raycast(ends[0].worldPosition, (ends[1].worldPosition - ends[0].worldPosition).normalized,
            out var hit1,
            (ends[1].worldPosition - ends[0].worldPosition).magnitude);
        bool rayCast2 = Physics.Raycast(ends[1].worldPosition, (ends[0].worldPosition - ends[1].worldPosition).normalized,
            out var hit2,
            (ends[0].worldPosition - ends[1].worldPosition).magnitude);
        if (!rayCast1 && !rayCast2) return;

        if (!hit1.rigidbody) return;

        if (hit1.collider.gameObject.CompareTag("Player")) return;

        hit1.rigidbody.freezeRotation = true;
        
        EndData newEnd1 = new EndData(hit1.rigidbody, hit1.rigidbody.transform.InverseTransformPoint(hit1.point));
        ends.Insert(1, newEnd1);
        
        SpringJoint newJoint1 = SetupJoint(newEnd1.rigidbody);
        newJoint1.anchor = ends[1].localPosition;
        newJoint1.connectedAnchor = ends[0].worldPosition;
        
        EndData newEnd2 = new EndData(hit2.rigidbody, hit2.rigidbody.transform.InverseTransformPoint(hit2.point));
        ends.Insert(2, newEnd2);
        
        SpringJoint newJoint2 = SetupJoint(newEnd2.rigidbody);
        newJoint2.anchor = ends[2].localPosition;
        newJoint2.connectedAnchor = ends[3].worldPosition;


        _foundHit = true;
    }

    private void UpdateDynamicIndexes()
    {
        foreach (EndData end in ends)
        {
            if (end.rigidbody) end.worldPosition = end.rigidbody.transform.TransformPoint(end.localPosition);
        }
    }

    private void DrawLine()
    {
        _lineRenderer.positionCount = ends.Count;
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, ends[i].worldPosition);
        }
    }
    
    public void PlaceEnd()
    {
        if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
                out var hit, PlayerData.Instance.rayCastDistance)) return;
        
        if (hit.collider.gameObject.CompareTag("Player")) return;

        EndData end = !hit.rigidbody ? new EndData(hit.point) : new EndData(hit.rigidbody, hit.rigidbody.transform.InverseTransformPoint(hit.point));

        ends.Add(end);

        if (ends.Count < 2) return;

        if (ends[0].rigidbody)
        {
            
            if (ends[1].rigidbody)
            {
                SpringJoint joint = SetupJoint(ends[1].rigidbody);

                joint.anchor = ends[1].localPosition;
                joint.connectedAnchor = ends[0].localPosition;
                
                joint.connectedBody = ends[0].rigidbody;
            }
            else
            {
                SpringJoint joint = SetupJoint(ends[0].rigidbody);
                
                joint.anchor = ends[0].localPosition;
                joint.connectedAnchor = ends[1].worldPosition;
            }
        }
        else
        {
            if (!ends[1].rigidbody) return;
            
            SpringJoint joint = SetupJoint(ends[1].rigidbody);

            joint.anchor = ends[1].localPosition;
            joint.connectedAnchor = ends[0].worldPosition;
        }
    }

    private SpringJoint SetupJoint(Rigidbody rb)
    {
        SpringJoint joint = rb.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.enableCollision = true;

        joint.maxDistance = PlayerData.Instance.maxRopeDistance;
        joint.minDistance = PlayerData.Instance.minRopeDistance;
                    
        joint.spring = PlayerData.Instance.springSpringiness * rb.mass;
        joint.damper = PlayerData.Instance.springDamper * rb.mass;
        joint.massScale = PlayerData.Instance.springMassScale;

        return joint;
    }
}