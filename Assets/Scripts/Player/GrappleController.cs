using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    // private void Update()
    // {
    //     if (PlayerData.Instance.joint && PlayerData.Instance.joint.connectedBody)
    //         PlayerData.Instance.firstEnd = PlayerData.Instance.joint.connectedBody.position;
    // }
    //
    // private void LateUpdate()
    // {
    //     DrawRope();
    // }
    //
    // public void StartGrapple()
    // {
    //     if (!Physics.Raycast(PlayerData.Instance.realCamera.position, PlayerData.Instance.realCamera.forward,
    //             out var hit, PlayerData.Instance.rayCastDistance)) return;
    //
    //     if (hit.collider.gameObject.CompareTag("Player") || !hit.rigidbody) return;
    //
    //     PlayerData.Instance.fixedJoint = PlayerData.Instance.gameObject.AddComponent<FixedJoint>();
    //     PlayerData.Instance.fixedJoint.connectedBody = hit.rigidbody;
    //
    //
    //     // if (hit.collider.gameObject.CompareTag("Player")) return;
    //
    //     // PlayerData.Instance.joint = PlayerData.Instance.gameObject.AddComponent<SpringJoint>();
    //     //     
    //     // if (hit.rigidbody)
    //     // {
    //     //     PlayerData.Instance.joint.connectedBody = hit.rigidbody;
    //     //     PlayerData.Instance.joint.autoConfigureConnectedAnchor = true;
    //     // }
    //     // else
    //     // {
    //     //     PlayerData.Instance.firstEnd = hit.point;
    //     //     PlayerData.Instance.joint.autoConfigureConnectedAnchor = false;
    //     //     PlayerData.Instance.joint.connectedAnchor = PlayerData.Instance.firstEnd;
    //     // }
    //     //
    //     // PlayerData.Instance.joint.maxDistance = PlayerData.Instance.maxRopeDistance;
    //     // PlayerData.Instance.joint.minDistance = PlayerData.Instance.minRopeDistance;
    //     //     
    //     // PlayerData.Instance.joint.spring = PlayerData.Instance.springSpringiness;
    //     // PlayerData.Instance.joint.damper = PlayerData.Instance.springDamper;
    //     // PlayerData.Instance.joint.massScale = PlayerData.Instance.springMassScale;
    //     //
    //     // PlayerData.Instance.lineRenderer.positionCount = 2;
    // }

    // public void StopGrapple()
    // {
    //     PlayerData.Instance.lineRenderer.positionCount = 0;
    //     if (PlayerData.Instance.joint)
    //     {
    //         Destroy(PlayerData.Instance.joint);
    //         PlayerData.Instance.joint = null;
    //     }
    //     
    //     if (PlayerData.Instance.fixedJoint)
    //     {
    //         Destroy(PlayerData.Instance.fixedJoint);
    //         PlayerData.Instance.fixedJoint = null;
    //     }
    // }
    //
    // private void DrawRope()
    // {
    //     if (!PlayerData.Instance.joint) return;
    //     
    //     PlayerData.Instance.lineRenderer.SetPosition(0, PlayerData.Instance.grapplePoint.position);
    //     PlayerData.Instance.lineRenderer.SetPosition(1, PlayerData.Instance.firstEnd);
    // }
}
