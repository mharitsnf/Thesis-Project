using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CutscenePlayer : MonoBehaviour
{
    public Cutscene relatedCutscene;
    public bool onlyPlayer; 
    public bool isLastTrigger;
    private int _onTriggerCounter;

    private void OnTriggerEnter(Collider other)
    {
        if (!relatedCutscene) return;
        if (other.gameObject.layer == 7) return;
        if (onlyPlayer && !other.gameObject.CompareTag("Player")) return;
        
        _onTriggerCounter++;

        if (relatedCutscene.isLastTriggerPlayed) return;
        if (isLastTrigger)
        {
            if (!relatedCutscene.atTargetPosition) relatedCutscene.StartCutscene();
            relatedCutscene.isLastTriggerPlayed = true;
        }
        else
        {
            if (_onTriggerCounter == 1) relatedCutscene.StartCutscene();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!relatedCutscene) return;
        if (other.gameObject.layer == 7) return;
        if (onlyPlayer && !other.gameObject.CompareTag("Player")) return;
        
        _onTriggerCounter--;

        if (relatedCutscene.isLastTriggerPlayed) return;
        if (isLastTrigger)
        {
            if (!relatedCutscene.atTargetPosition) relatedCutscene.StartCutscene();
            relatedCutscene.isLastTriggerPlayed = true;
        }
        else
        {
            if (_onTriggerCounter == 0) relatedCutscene.StartCutscene();
        }
    }
}
