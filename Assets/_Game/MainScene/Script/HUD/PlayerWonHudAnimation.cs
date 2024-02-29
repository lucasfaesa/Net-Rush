using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerWonHudAnimation : MonoBehaviour
{
    [Header("SO's")] 
    [SerializeField] private HudEventsChannelSO hudEventsChannel;
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private GamePointsSO gamePoints;
    [Header("Refs")] 
    [SerializeField] private Transform playerWonWindow;
    [SerializeField] private List<Transform> stripes = new();
    [SerializeField] private TextMeshProUGUI playerWonText;

    private Sequence showInterfaceSequence;
    private Sequence showAndScaleTextSequence;
    private Sequence hideInterfaceSequence;
    
    private void OnEnable()
    {
        gameEventsChannel.GameEnded += ShowPlayerWonScreen;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameEnded -= ShowPlayerWonScreen;
    }
    
    private void ShowPlayerWonScreen()
    {
        hudEventsChannel.OnGameWonHudAnimationStarted();
        StartCoroutine(ShowPlayerWonScreenRoutine());
    }

    private IEnumerator ShowPlayerWonScreenRoutine()
    {
        SetObjectsOutOfScreen();
        int leftPlayerPoints = gamePoints.GameCurrentPoints.leftPlayerPoints;
        int rightPlayerPoints = gamePoints.GameCurrentPoints.rightPlayerPoints;

        playerWonText.text = leftPlayerPoints > rightPlayerPoints ? "Player One\nWins!" :
            (leftPlayerPoints < rightPlayerPoints) ? "Player Two\nWins" : "Draw";
        
        yield return new WaitForSeconds(1.3f);
        
        playerWonWindow.gameObject.SetActive(true);


        showInterfaceSequence = DOTween.Sequence()
            .Append(stripes[0].DOLocalMoveY(0f, 0.8f).SetEase(Ease.OutBack))
            .Insert(0, stripes[1].DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack))
            .Insert(0, stripes[2].DOLocalMoveY(0f, 1.2f).SetEase(Ease.OutBack)).AppendInterval(0.07f)
            .AppendCallback(()=>
            {
                playerWonText.gameObject.SetActive(true);
            })
            .Append(playerWonText.transform.DOScale(1.2f, 3f))
            .AppendCallback(() =>
            {
                playerWonText.gameObject.SetActive(false);
            })
            .Append(stripes[0].DOLocalMoveY(10000f, 0.7f).SetEase(Ease.InBack))
            .Insert(4.3f, stripes[1].DOLocalMoveY(-10000f, 0.7f).SetEase(Ease.InBack))
            .Insert(4.3f, stripes[2].DOLocalMoveY(10000f, 0.7f).SetEase(Ease.InBack))
            .OnComplete(() =>
            {
                stripes.ForEach(x=>x.gameObject.SetActive(false));
                hudEventsChannel.OnGameWonHudAnimationEnded();
            });
        

    }

    private void SetObjectsOutOfScreen()
    {
        stripes[0].transform.localPosition = new Vector3(0f, 10000f, 0f);
        stripes[1].transform.localPosition = new Vector3(0f, -10000f, 0f);
        stripes[2].transform.localPosition = new Vector3(0f, 10000f, 0f);
        
        stripes.ForEach(x=>x.gameObject.SetActive(true));
    }
}
