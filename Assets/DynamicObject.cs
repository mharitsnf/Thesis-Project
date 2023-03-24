using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    public float smoothness = 10;
    public Vector3 targetPosition;

    public IEnumerator MovePosition()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) transform.position = targetPosition;
            
            yield return null;
        }
    }
}
