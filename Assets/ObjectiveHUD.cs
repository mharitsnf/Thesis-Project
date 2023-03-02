using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveHUD : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    public static ObjectiveHUD Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHUD()
    {
        _textMeshProUGUI.text = "Objective collected: " + PlayerData.Instance.objectiveCollectedAmount;
    }
}
