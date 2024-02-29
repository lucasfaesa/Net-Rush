using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventsChannel", menuName = "ScriptableObjects/Game/GameEventsChannel", order = 1)]
public class GameEventsChannelSO : ScriptableObject
{
    public event Action SceneLoaded;
    public event Action InitialCountdownToStart;
    public event Action<int> InitialCountdownUpdated;
    public event Action InitialCountdownEnded;
    public event Action GameStarted;

    public event Action GameStopwatchStarted;
    public event Action GameStopwatchEnded;

    public event Action GameEnded;
    
    public event Action<PlayerStatsSO.PlayerSideEnum> PlayerReadyToServe;
    
    
    public void OnSceneLoaded()
    {
        SceneLoaded?.Invoke();
    }

    public void OnInitialCountdownToStart()
    {
        InitialCountdownToStart?.Invoke();
    }

    public void OnInitialCountdownUpdated(int time)
    {
        InitialCountdownUpdated?.Invoke(time);
    }

    public void OnInitialCountdownEnded()
    {
        InitialCountdownEnded?.Invoke();
    }

    public void OnGameStarted()
    {
        GameStarted?.Invoke();
    }

    public void OnPlayerReadyToServe(PlayerStatsSO.PlayerSideEnum playerSide)
    {
        PlayerReadyToServe?.Invoke(playerSide);
    }
    
    public void OnGameStopwatchStarted()
    {
        GameStopwatchStarted?.Invoke();
    }

    public void OnGameStopwatchEnded()
    {
        GameStopwatchEnded?.Invoke();
    }

    public void OnGameEnded()
    {
        GameEnded?.Invoke();
    }
    
}
