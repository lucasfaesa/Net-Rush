using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallEventsChannel", menuName = "ScriptableObjects/Ball/BallEventsChannel", order = 1)]
public class BallEventsChannelSO : ScriptableObject
{
    public enum FieldSideEnum{LeftSide, RightSide}
    
    //last player who touched the ball, field side the ball fell    
    public event Action<PlayerStatsSO.PlayerSideEnum, FieldSideEnum> BallIn;
    public event Action<PlayerStatsSO.PlayerSideEnum> BallOut;
    
    public void OnBallOut(PlayerStatsSO.PlayerSideEnum playerSide)
    {
        BallOut?.Invoke(playerSide);
    }

    public void OnBallIn(PlayerStatsSO.PlayerSideEnum playerSide, FieldSideEnum fieldSide)
    {
        BallIn?.Invoke(playerSide, fieldSide);
    }
}
