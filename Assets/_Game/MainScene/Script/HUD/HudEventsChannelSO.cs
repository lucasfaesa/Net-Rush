using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HudEventsChannel", menuName = "ScriptableObjects/HUD/HudEventsChannel", order = 1)]
public class HudEventsChannelSO : ScriptableObject
{
    public event Action GameWonHudAnimationStarted;
    public event Action GameWonHudAnimationEnded;
    
    public void OnGameWonHudAnimationStarted()
    {
        GameWonHudAnimationStarted?.Invoke();
    }

    public void OnGameWonHudAnimationEnded()
    {
        GameWonHudAnimationEnded?.Invoke();
    }
}
