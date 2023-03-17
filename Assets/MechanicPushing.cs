using System;
using UnityEngine;

public class MechanicPushing : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Object")) return;
        
        GameObject otherGameObject = other.gameObject;
        
        otherGameObject.GetComponent<Rigidbody>().AddForce(PlayerData.Instance.rigidBody.velocity.normalized * PlayerData.Instance.pushForce, ForceMode.Acceleration);
    }
}
