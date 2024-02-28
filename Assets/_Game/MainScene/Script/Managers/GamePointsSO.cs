using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePoints", menuName = "ScriptableObjects/Game/GamePoints")]
public class GamePointsSO : ScriptableObject
{
    public event Action<int, int> PointsUpdated;
    public event Action<PlayerStatsSO.PlayerSideEnum> PlayerScored;

    public (int leftPlayerPoints, int rightPlayerPoints) GameCurrentPoints { get; set; }



    public void AddPoints(PlayerStatsSO.PlayerSideEnum side)
    {
        if (side == PlayerStatsSO.PlayerSideEnum.LeftSide)
        {
            GameCurrentPoints = (GameCurrentPoints.leftPlayerPoints + 1, GameCurrentPoints.rightPlayerPoints);
            OnPlayerScored(PlayerStatsSO.PlayerSideEnum.LeftSide);
        }

        if (side == PlayerStatsSO.PlayerSideEnum.RightSide)
        {
            GameCurrentPoints = (GameCurrentPoints.leftPlayerPoints, GameCurrentPoints.rightPlayerPoints + 1);
            OnPlayerScored(PlayerStatsSO.PlayerSideEnum.RightSide);
        }
        
        OnPointsUpdated(GameCurrentPoints.leftPlayerPoints, GameCurrentPoints.rightPlayerPoints);
    }
    
    private void OnPointsUpdated(int leftPlayerPoints, int rightPlayerPoints)
    {
        GameCurrentPoints = (leftPlayerPoints, rightPlayerPoints);
        PointsUpdated?.Invoke(GameCurrentPoints.leftPlayerPoints, GameCurrentPoints.rightPlayerPoints);
        
        Debug.Log($"Left Player Points: {GameCurrentPoints.leftPlayerPoints}, Right Player Points: {GameCurrentPoints.rightPlayerPoints}");
    }

    private void OnPlayerScored(PlayerStatsSO.PlayerSideEnum player)
    {
        PlayerScored?.Invoke(player);
    }

    public void Reset()
    {
        GameCurrentPoints = (0, 0);
    }
}
