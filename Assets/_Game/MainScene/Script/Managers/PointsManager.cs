using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [Header("So's")] 
    [SerializeField] private BallEventsChannelSO ballEventsChannel;
    [SerializeField] private GamePointsSO gamePoints;
    [Header("Text References")] 
    [SerializeField] private TextMeshPro leftPlayerScore;
    [SerializeField] private TextMeshPro rightPlayerScore;
    
    private void OnEnable()
    {
        ballEventsChannel.BallIn += UpdatePoints;
        ballEventsChannel.BallOut += UpdatePoints;
        gamePoints.Reset();
    }

    private void OnDisable()
    {
        ballEventsChannel.BallIn -= UpdatePoints;
        ballEventsChannel.BallOut -= UpdatePoints;
    }

    private void UpdatePoints(PlayerStatsSO.PlayerSideEnum playerSideEnum, BallEventsChannelSO.FieldSideEnum fieldSideEnum)
    {
        if ((playerSideEnum == PlayerStatsSO.PlayerSideEnum.RightSide && fieldSideEnum == BallEventsChannelSO.FieldSideEnum.RightSide) ||
            (playerSideEnum == PlayerStatsSO.PlayerSideEnum.LeftSide && fieldSideEnum == BallEventsChannelSO.FieldSideEnum.RightSide))
        {
            gamePoints.AddPoints(PlayerStatsSO.PlayerSideEnum.LeftSide);
        }
        
        if ((playerSideEnum == PlayerStatsSO.PlayerSideEnum.LeftSide && fieldSideEnum == BallEventsChannelSO.FieldSideEnum.LeftSide) ||
                 (playerSideEnum == PlayerStatsSO.PlayerSideEnum.RightSide && fieldSideEnum == BallEventsChannelSO.FieldSideEnum.LeftSide))
        {
            gamePoints.AddPoints(PlayerStatsSO.PlayerSideEnum.RightSide);
        }

        UpdateWorldScore();
    }
    
    private void UpdatePoints(PlayerStatsSO.PlayerSideEnum playerSideEnum)
    {
        if(playerSideEnum == PlayerStatsSO.PlayerSideEnum.LeftSide)
            gamePoints.AddPoints(PlayerStatsSO.PlayerSideEnum.RightSide);
        
        if(playerSideEnum == PlayerStatsSO.PlayerSideEnum.RightSide)
            gamePoints.AddPoints(PlayerStatsSO.PlayerSideEnum.LeftSide);

        UpdateWorldScore();
    }

    private void UpdateWorldScore()
    {
        leftPlayerScore.text = gamePoints.GameCurrentPoints.leftPlayerPoints.ToString();
        rightPlayerScore.text = gamePoints.GameCurrentPoints.rightPlayerPoints.ToString();
    }
}
