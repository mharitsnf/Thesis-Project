using System;
using UnityEngine;

public abstract class PlayerBaseManager : MonoBehaviour
{
    [Header("Player Data")]
    [HideInInspector] public Transform meshes;
    [HideInInspector] public Transform orientation;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public CinemachineFollowController cinemachineFollowController;
    [HideInInspector] public InputHandler inputHandler;
    
    public PlayerBaseState currentState;
    public PlayerBaseState previousState;

    protected void Awake()
    {
        SetPlayerData();
    }

    private void SetPlayerData()
    {
        meshes = transform.GetChild(0);
        orientation = transform.GetChild(1);
        rigidBody = GetComponent<Rigidbody>();
        cinemachineFollowController = GetComponent<CinemachineFollowController>();
        inputHandler = GetComponent<InputHandler>();
    }

    public virtual void SwitchState(PlayerBaseState state)
    {
        currentState = state;
    }
}
