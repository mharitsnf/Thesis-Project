using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKScript : MonoBehaviour
{

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_animator) return;

        float maxDistance = PlayerData.Instance.feetDistanceToGround;

        if (!Physics.Raycast(new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down), out var hit, maxDistance))
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
            return;
        }

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, hit.distance / maxDistance);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, hit.distance / maxDistance);
        
        Vector3 footPosition = hit.point;
        footPosition.y += PlayerData.Instance.feetDistanceToGround;
        _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
        _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        
        if (!Physics.Raycast(new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot), Vector3.down), out hit, maxDistance))
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            return;
        }
        
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, hit.distance / maxDistance);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, hit.distance / maxDistance);
        
        footPosition = hit.point;
        footPosition.y += PlayerData.Instance.feetDistanceToGround;
        _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
        _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
    }
}
