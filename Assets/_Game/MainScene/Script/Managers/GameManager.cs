using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("SO's")]
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private PlayerStatsSO playerLeftStats;
    [SerializeField] private PlayerStatsSO playerRightStats;
    [SerializeField] private BallStatsSO ballStats;
    [SerializeField] private BallEventsChannelSO ballEventsChannel;
    [SerializeField] private GamePointsSO gamePoints;
    [Space] 
    [Header("Scripts Refs")]
    [SerializeField] private PlayersInstantiator playersInstantiator;
    [Header("Ball")] 
    [SerializeField] private BallBehavior ballPrefab;
    
    
    private PlayerController _playerLeft;
    private PlayerController _playerRight;

    private BallBehavior _ball;

    private void OnEnable()
    {
        gamePoints.PlayerScored += OnPlayerScored;
        gameEventsChannel.GameStopwatchEnded += GameEnded;
    }

    private void OnDisable()
    {
        gamePoints.PlayerScored -= OnPlayerScored;
        gameEventsChannel.GameStopwatchEnded -= GameEnded;
    }

    private void Awake()
    {
        gameEventsChannel.OnSceneLoaded();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        playersInstantiator.InstantiatePlayers(ref _playerLeft, ref _playerRight);
        _ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        
        gameEventsChannel.OnInitialCountdownToStart();
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float waitTime = 3f;
        float elapsedTime = 0;
        
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            gameEventsChannel.OnInitialCountdownUpdated((int)elapsedTime);
            yield return null; 
        }
        
        gameEventsChannel.OnInitialCountdownEnded();
        gameEventsChannel.OnGameStarted();
        
        StartCoroutine(LeftPlayerServe());
    }

    private IEnumerator LeftPlayerServe()
    {
        yield return new WaitForSeconds(1f);
        
        var playerLeftPos = _playerLeft.transform.position;
        _playerLeft.transform.position = new Vector3(playerLeftPos.x, playerLeftPos.y, playerLeftStats.MinZPosValueLeftSide);
        
        _ball.ResetAndLockBall(ballStats.LeftPlayerBallServePosition);
        _ball.gameObject.SetActive(true);
        
        gameEventsChannel.OnPlayerReadyToServe(PlayerStatsSO.PlayerSideEnum.LeftSide);
    }
    
    private IEnumerator RightPlayerServe()
    {
        yield return new WaitForSeconds(1f);
        
        var playerRightPos = _playerRight.transform.position;
        _playerRight.transform.position = new Vector3(playerRightPos.x, playerRightPos.y, playerRightStats.MaxZPosValueRightSide);
        
        _ball.ResetAndLockBall(ballStats.RightPlayerBallServePosition);
        _ball.gameObject.SetActive(true);
        
        gameEventsChannel.OnPlayerReadyToServe(PlayerStatsSO.PlayerSideEnum.RightSide);
    }

    private void OnPlayerScored(PlayerStatsSO.PlayerSideEnum playerSideEnum)
    {
        if (playerSideEnum == PlayerStatsSO.PlayerSideEnum.LeftSide)
            StartCoroutine(LeftPlayerServe());
        
        if (playerSideEnum == PlayerStatsSO.PlayerSideEnum.RightSide)
            StartCoroutine(RightPlayerServe());
    }

    private void GameEnded()
    {
        gameEventsChannel.OnGameEnded();
    }
    
}
