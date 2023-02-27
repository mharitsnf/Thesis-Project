using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectMechanicsController : MonoBehaviour
{
    private Rigidbody _rb;
    
    private void Awake()
    {
        gameObject.tag = "Object";

        try
        {
            Assert.IsTrue(GetComponent<Rigidbody>());
            Assert.IsTrue(GetComponent<Collider>());
        }
        catch (Exception)
        {
            Debug.Break();
            throw;
        }

        _rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (GetComponents<SpringJoint>().Length == 0) _rb.freezeRotation = false;
    }
}
