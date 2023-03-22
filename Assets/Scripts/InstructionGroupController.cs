using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InstructionGroupController : MonoBehaviour
{
    public static InstructionGroupController Instance { get; private set; }
    
    public GameObject instructionPrefab;
    private bool isShown;

    public bool IsShown
    {
        get => isShown;
        set
        {
            isShown = value;
            if (!value) RemoveChildren();
        }
    }

    public enum DisplayState
    {
        NotAiming, ObjectNotSelected, ObjectSelected
    }

    private DisplayState _currentState;

    public DisplayState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            
            if (!isShown) return;
            
            List<String> instructions = new();
            switch (value)
            {
                case DisplayState.NotAiming:
                    instructions.Add("[Right Click] Aim");
                    if (PlayerData.Instance.placedRopes.Count > 0) instructions.Add("[E] Detach Newest Rope");
                    if (PlayerData.Instance.placedRopes.Count > 0) instructions.Add("[Q] Detach Oldest Rope");
                    SetInstructions(instructions);
                    break;
                case DisplayState.ObjectNotSelected:
                    instructions.Add("[Right Click]/[Q] Exit");
                    instructions.Add("[Left Click] Select Object");
                    SetInstructions(instructions);
                    break;
                case DisplayState.ObjectSelected:
                    instructions.Add("[Right Click]/[Q] Exit");
                    instructions.Add("[Left Click] Select Point");
                    instructions.Add("[R] Confirm");
                    SetInstructions(instructions);
                    break;
            }
        }
    }

    private void SetInstructions(List<String> instructions)
    {
        RemoveChildren();

        foreach (String instruction in instructions)
        {
            GameObject instructionObject = Instantiate(instructionPrefab, transform);
            instructionObject.GetComponent<TextMeshProUGUI>().SetText(instruction);
        }
    }

    private void RemoveChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        CurrentState = DisplayState.NotAiming;
    }
}
