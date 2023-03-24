using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Objective : MonoBehaviour
{

    public int ignoreLayerIndex;
    public GameObject captureParticlesPrefab;
    public Cutscene relatedCutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ignoreLayerIndex) return;
        if (!other.gameObject.CompareTag("Player")) return;

        PlayerData.Instance.objectiveCollectedAmount++;
        ObjectiveHUD.Instance.UpdateHUD();

        Instantiate(captureParticlesPrefab, transform.position, Quaternion.identity);
        
        if (relatedCutscene && relatedCutscene.playLimit > 0) relatedCutscene.StartCutscene();

        GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);
    }

}
