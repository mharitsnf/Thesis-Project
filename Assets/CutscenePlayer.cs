using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    public Cutscene relatedCutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) return;
        if (!other.gameObject.CompareTag("Player")) return;
        if (relatedCutscene && relatedCutscene.playLimit > 0) relatedCutscene.StartCutscene();
    }
}
