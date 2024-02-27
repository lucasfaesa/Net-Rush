using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventsChannel", menuName = "ScriptableObjects/Game/GameEventsChannel", order = 1)]
public class GameEventsChannelSO : ScriptableObject
{
    public event Action SceneLoaded;
    public event Action CountdownToStart;
    public event Action<int> CountdownUpdated;
    public event Action CountdownEnded;
    public event Action GameStarted;

    public event Action<PlayerStatsSO.PlayerSideEnum> PlayerGoingToServe; 
    public event Action<PlayerStatsSO.PlayerSideEnum> PlayerReadyToServe;
    
    
    public void OnSceneLoaded()
    {
        SceneLoaded?.Invoke();
    }

    public void OnCountdownToStart()
    {
        CountdownToStart?.Invoke();
    }

    public void OnCountdownUpdated(int time)
    {
        CountdownUpdated?.Invoke(time);
    }

    public void OnCountdownEnded()
    {
        CountdownEnded?.Invoke();
    }

    public void OnGameStarted()
    {
        GameStarted?.Invoke();
    }
    
    public void OnPlayerGoingToServe(PlayerStatsSO.PlayerSideEnum playerSide)
    {
        PlayerGoingToServe?.Invoke(playerSide);
    }

    public void OnPlayerReadyToServe(PlayerStatsSO.PlayerSideEnum playerSide)
    {
        PlayerReadyToServe?.Invoke(playerSide);
    }
    
}
