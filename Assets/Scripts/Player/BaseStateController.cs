using System;
using UnityEngine;

public abstract class BaseStateController : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    public PlayerBaseState currentState;
    public PlayerBaseState previousState;

    protected void Awake()
    {
        SetPlayerData();
    }

    private void SetPlayerData()
    {
        playerData = GetComponent<PlayerData>();
    }

    public virtual void SwitchState(PlayerBaseState state)
    {
        currentState = state;
    }
}
