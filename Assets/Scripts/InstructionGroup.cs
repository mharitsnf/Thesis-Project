using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InstructionGroup : MonoBehaviour
{
    public static InstructionGroup Instance { get; private set; }
    
    public GameObject instructionPrefab;

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
            List<String> instructions = new();
            switch (value)
            {
                case DisplayState.NotAiming:
                    instructions.Add("Right Click to Aim");
                    if (PlayerData.Instance.placedRopes.Count > 0) instructions.Add("Q to Detach Latest Rope");
                    if (PlayerData.Instance.placedRopes.Count > 0) instructions.Add("LShift to Detach Earliest Rope");
                    SetInstructions(instructions);
                    break;
                case DisplayState.ObjectNotSelected:
                    instructions.Add("Right Click/Q/E to Exit");
                    instructions.Add("Left Click to Select Object");
                    SetInstructions(instructions);
                    break;
                case DisplayState.ObjectSelected:
                    instructions.Add("Right Click/Q to Exit");
                    instructions.Add("Left Click to Select Point");
                    instructions.Add("E to Confirm");
                    SetInstructions(instructions);
                    break;
                default:
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
