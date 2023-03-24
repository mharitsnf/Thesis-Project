using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicObject : MonoBehaviour
{
    public float smoothness = 10;
    private float _noise;
    public Vector3 targetPosition;
    private Vector3 _initialPosition;

    private void Start()
    {
        _noise = Random.Range(-1, 1);
        _initialPosition = transform.position;
    }

    public IEnumerator MovePosition()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * (smoothness + _noise));

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) transform.position = targetPosition;
            
            yield return null;
        }
    }
}
