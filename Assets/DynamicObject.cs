using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicObject : MonoBehaviour
{
    public float smoothness = 10;
    public Vector3 targetPosition;
    public bool atTargetPosition;
    
    private float _noise;
    private Vector3 _initialPosition;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _noise = Random.Range(-1, 1);
        _initialPosition = transform.position;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }
    
    public IEnumerator MovePosition(bool toTarget)
    {
        Vector3 nextPos = toTarget ? targetPosition : _initialPosition;
        
        while (Vector3.Distance(transform.position, nextPos) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * (smoothness + _noise));

            if (Vector3.Distance(transform.position, nextPos) < 0.1f) transform.position = nextPos;
            
            yield return null;
        }

        if (toTarget) atTargetPosition = true;
    }
}
