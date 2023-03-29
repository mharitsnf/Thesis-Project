using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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

    public SpringJoint joint;
    public GameObject attachmentPointPrefab;
    public GameObject attachmentPoint;
    public Material[] materials;
    
    
    public EndData firstEnd;
    public EndData secondEnd;
    private LineRenderer _lineRenderer;
    public bool canDraw;


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
        if (firstEnd != null && firstEnd.rigidbody) firstEnd.worldPosition = firstEnd.rigidbody.transform.TransformPoint(firstEnd.localPosition);
        if (secondEnd != null && secondEnd.rigidbody) secondEnd.worldPosition = secondEnd.rigidbody.transform.TransformPoint(secondEnd.localPosition);
    }

    private void DrawLine()
    {
        if (!canDraw) return;
        
        if (firstEnd != null)
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, firstEnd.rigidbody ? firstEnd.rigidbody.transform.position : firstEnd.worldPosition);
        }

        if (secondEnd != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(1, secondEnd.rigidbody ? secondEnd.rigidbody.transform.position : secondEnd.worldPosition);
        }
    }

    public bool PlaceEnd(RaycastHit hit)
    {
        if (firstEnd != null && !firstEnd.rigidbody && !hit.rigidbody) return false;
        if (firstEnd != null && firstEnd.rigidbody == hit.rigidbody) return false;
        
        EndData end = (hit.rigidbody && !hit.rigidbody.isKinematic) ? new EndData(hit.rigidbody, hit.rigidbody.transform.InverseTransformPoint(hit.point), hit.point) :  new EndData(hit.point);

        if (firstEnd == null) firstEnd = end;
        else
        {
            secondEnd = end;
            attachmentPoint = Instantiate(attachmentPointPrefab, end.worldPosition, quaternion.identity);
        }
        
        return true;
    }

    public void CreateJoint()
    {
        if (firstEnd.rigidbody && secondEnd.rigidbody)
        {
            firstEnd.rigidbody.freezeRotation = true;
            secondEnd.rigidbody.freezeRotation = true;
            joint = SetupJoint(firstEnd.rigidbody, (firstEnd.worldPosition - secondEnd.worldPosition).magnitude);

            joint.connectedBody = secondEnd.rigidbody;
            joint.anchor = firstEnd.localPosition;
            joint.connectedAnchor = secondEnd.localPosition;
        }
        else if (firstEnd.rigidbody && !secondEnd.rigidbody)
        {
            firstEnd.rigidbody.freezeRotation = true;
            joint = SetupJoint(firstEnd.rigidbody, (firstEnd.worldPosition - secondEnd.worldPosition).magnitude);
        
            joint.connectedAnchor = secondEnd.worldPosition;
        }

        Destroy(attachmentPoint);
        canDraw = true;
    }

    public void SetColor(int index)
    {
        _lineRenderer.material = materials[index];
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