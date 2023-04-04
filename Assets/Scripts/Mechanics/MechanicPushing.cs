using System;
using System.Collections;
using UnityEngine;

public class MechanicPushing : MonoBehaviour
{
    private readonly float _maxDistance = .3f;
    private float _currentWeight;
    
    private void OnTriggerStay(Collider other)
    {
        if (!Physics.Raycast(transform.position, transform.forward, out var hit, _maxDistance)) return;
        if (hit.collider.isTrigger) return;
        if (_currentWeight < 1f) _currentWeight = Mathf.Lerp(_currentWeight, 1.1f, Time.deltaTime * PlayerData.Instance.pushingAnimationSmoothness);
        PlayerData.Instance.animator.SetLayerWeight(1, _currentWeight);
        
        if (!other.GetComponent<Rigidbody>()) return;
        
        GameObject otherGameObject = other.gameObject;
        otherGameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * PlayerData.Instance.pushForce, hit.point, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ResetWeight());
    }

    IEnumerator ResetWeight()
    {
        while (_currentWeight > 0)
        { 
            _currentWeight = Mathf.Lerp(_currentWeight, -.1f, Time.deltaTime * PlayerData.Instance.pushingAnimationSmoothness);
            PlayerData.Instance.animator.SetLayerWeight(1, _currentWeight);
            yield return null;
        }
    }
}
