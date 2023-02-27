using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectMechanicsController : MonoBehaviour
{
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
    }
}
