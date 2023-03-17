using System;
using UnityEngine;

public class MechanicPushing : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Object")) return;
        if (!Physics.Raycast(PlayerData.Instance.meshes.position, PlayerData.Instance.meshes.forward, out var hit, 2f)) return;
        
        GameObject otherGameObject = other.gameObject;
        
        otherGameObject.GetComponent<Rigidbody>().AddForce(- hit.normal * PlayerData.Instance.pushForce, ForceMode.Acceleration);
    }
}
