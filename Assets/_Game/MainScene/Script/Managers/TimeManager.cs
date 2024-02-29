using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("SO's")]
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [Header("Time Settings")] 
    [SerializeField] private float gameDuration = 120f;
    [Header("Hourglass Material References")]
    [SerializeField] private Renderer topSandRenderer;
    [SerializeField] private Renderer bottomSandRenderer;
    [SerializeField] private string materializeProperty = "_Materialize";
    [Header("Hourglass Particle Reference")] 
    [SerializeField] private ParticleSystem sandParticle;
    
    private void OnEnable()
    {
        gameEventsChannel.GameStarted += StartStopwatch;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameStarted -= StartStopwatch;
    }

    private void StartStopwatch()
    {
        gameEventsChannel.OnGameStopwatchStarted();
        StartCoroutine(StartGameTimer(gameDuration));
        sandParticle.Play();
        Materialize(bottomSandRenderer.material, gameDuration);
        DeMaterialize(topSandRenderer.material, gameDuration);
        
    }

    private IEnumerator StartGameTimer(float timer)
    {
        yield return new WaitForSeconds(gameDuration);
        
        sandParticle.Stop();
        gameEventsChannel.OnGameStopwatchEnded();
    }
    
    private void Materialize(Material mat, float time)
    {
        DOTween.To(x => mat.SetFloat(materializeProperty, x), 0, 1, time).SetEase(Ease.Linear);
    }

    private void DeMaterialize(Material mat, float time)
    {
        DOTween.To(x => mat.SetFloat(materializeProperty, x), 1, 0, time).SetEase(Ease.Linear);
    }
    
    public void Reset()
    {
        bottomSandRenderer.material.SetFloat(materializeProperty, 1);
        topSandRenderer.material.SetFloat(materializeProperty, 0);
    }

       
     
}
