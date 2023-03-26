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
        if (_onTriggerCounter > 0) return;
        
        if (relatedCutscene.playLimit < 0)
        {
            if (isLastTrigger && !relatedCutscene.atTargetPosition) relatedCutscene.StartCutscene();
            else
            {
                relatedCutscene.StartCutscene();
            }
        }
        else
        {
            if (relatedCutscene.playLimit > 0) relatedCutscene.StartCutscene();
        }

        _onTriggerCounter++;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!relatedCutscene) return;
        
        _onTriggerCounter--;
        
        if (_onTriggerCounter > 0) return;
        if (!relatedCutscene) return;
        if (other.gameObject.layer == 7) return;
        if (onlyPlayer)
        {
            if (!other.gameObject.CompareTag("Player")) return;
        }

        if (true)
        {
            if (isLastTrigger && !relatedCutscene.atTargetPosition) relatedCutscene.StartCutscene();
            else
            {
                relatedCutscene.StartCutscene();
            }
        }
        else
        {
            if (relatedCutscene.playLimit > 0) relatedCutscene.StartCutscene();
        }
        
    }
}
