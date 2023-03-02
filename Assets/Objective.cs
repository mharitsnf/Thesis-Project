using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Objective : MonoBehaviour
{

    public int ignoreLayerIndex;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ignoreLayerIndex) return;
        if (!other.gameObject.CompareTag("Player")) return;

        PlayerData.Instance.objectiveCollectedAmount++;
        ObjectiveHUD.Instance.UpdateHUD();
        
        GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);
    }

}
