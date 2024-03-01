using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldTimescaleController : MonoBehaviour
{
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private AnimationFeedbackEventChannelSO LeftPlayerAnimationFeedback;
    [SerializeField] private AnimationFeedbackEventChannelSO RightPlayerAnimationFeedback;
    [Space] 
    [SerializeField] private PlayersInstantiator playersInstantiator;

    private PlayerController _leftPlayer;
    private PlayerController _rightPlayer;

    private PlayerActionsController.CutPowerEnum currentLeftPlayerCutPower;
    private PlayerActionsController.CutPowerEnum currentRightPlayerCutPower;

    private CinemachineVirtualCamera leftPlayerCamera;
    private CinemachineVirtualCamera rightPlayerCamera;
    
    private void OnEnable()
    {
        gameEventsChannel.GameStarted += GameStarted;
        LeftPlayerAnimationFeedback.CutPowerChanged += LeftPlayerCurrentPowerChanged;
        RightPlayerAnimationFeedback.CutPowerChanged += RightPlayerCurrentPowerChanged;
        LeftPlayerAnimationFeedback.CutWideTriggered += OnLeftPlayerCut;
        RightPlayerAnimationFeedback.CutWideTriggered += OnRightPlayerCut;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameStarted -= GameStarted;
        LeftPlayerAnimationFeedback.CutPowerChanged -= LeftPlayerCurrentPowerChanged;
        RightPlayerAnimationFeedback.CutPowerChanged -= RightPlayerCurrentPowerChanged;
        LeftPlayerAnimationFeedback.CutWideTriggered -= OnLeftPlayerCut;
        RightPlayerAnimationFeedback.CutWideTriggered -= OnRightPlayerCut;
    }

    private void GameStarted()
    {
        _leftPlayer = playersInstantiator.LeftPlayer;
        leftPlayerCamera = _leftPlayer.GetComponentInChildren<CinemachineVirtualCamera>();
        _rightPlayer = playersInstantiator.RightPlayer;
        rightPlayerCamera = _rightPlayer.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void LeftPlayerCurrentPowerChanged(PlayerActionsController.CutPowerEnum cutPowerEnum)
    {
        currentLeftPlayerCutPower = cutPowerEnum;
    }
    
    private void RightPlayerCurrentPowerChanged(PlayerActionsController.CutPowerEnum cutPowerEnum)
    {
        currentRightPlayerCutPower = cutPowerEnum;
    }
    
    private void OnLeftPlayerCut(Rigidbody _)
    {
        if (Random.Range(1, 11) != 1)
            return;
            
        if (currentLeftPlayerCutPower == PlayerActionsController.CutPowerEnum.VeryStrong)
        {
            StartCoroutine(PlayerCutRoutine(leftPlayerCamera));
        }
    }
    
    private void OnRightPlayerCut(Rigidbody _)
    {
        if (Random.Range(1, 11) != 1)
            return;
        
        if (currentRightPlayerCutPower == PlayerActionsController.CutPowerEnum.VeryStrong)
        {
            StartCoroutine(PlayerCutRoutine(rightPlayerCamera));
        }
    }
    
    private IEnumerator PlayerCutRoutine(CinemachineVirtualCamera camera)
    {
        Time.timeScale = 0f;
        camera.enabled = true;

        yield return new WaitForSecondsRealtime(2f);
        camera.enabled = false;
        
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f;
    }
}
