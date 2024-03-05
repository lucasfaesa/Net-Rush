using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FeedbackText : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [Space]
    [SerializeField] private TextMeshPro feedbackText;
    
    private Sequence animSequence;
    private Transform parent;
    
    private void OnEnable()
    {
        animationFeedbackEventChannel.CutPowerChanged += UpdateCutPower;
        animationFeedbackEventChannel.CutFarTriggered += AnimateText;
        animationFeedbackEventChannel.CutWideTriggered += AnimateText;
        animationFeedbackEventChannel.CutNarrowTriggered += AnimateText;
    }

    private void OnDisable()
    {
        animationFeedbackEventChannel.CutPowerChanged -= UpdateCutPower;
        animationFeedbackEventChannel.CutFarTriggered -= AnimateText;
        animationFeedbackEventChannel.CutWideTriggered -= AnimateText;
        animationFeedbackEventChannel.CutNarrowTriggered -= AnimateText;
    }

    private void Awake()
    {
        parent = this.transform.parent;
        
        if (animSequence == null)
            animSequence = DOTween.Sequence()
                .Append(feedbackText.transform.DOLocalMoveY(0.35f, 0.9f).SetEase(Ease.Linear))
                .Insert(0, feedbackText.DOFade(0f, 0.9f).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    feedbackText.gameObject.SetActive(false);
                })
                .SetAutoKill(false).Pause();
    }

    private void UpdateCutPower(PlayerActionsController.CutPowerEnum cutPowerEnum)
    {
        if (animSequence.IsPlaying()) return;
        
        switch (cutPowerEnum)
        {
            case PlayerActionsController.CutPowerEnum.None:
                feedbackText.text = "Miss";
                feedbackText.color = Color.white;
                break;
            case PlayerActionsController.CutPowerEnum.Weak:
                feedbackText.text = "Early";
                feedbackText.color = Color.red;
                break;
            case PlayerActionsController.CutPowerEnum.Strong:
                feedbackText.text = "Late";
                feedbackText.color = Color.yellow;
                break;
            case PlayerActionsController.CutPowerEnum.VeryStrong:
                feedbackText.text = "Perfect!";
                feedbackText.color = Color.green;
                break;
        }
    }

    private void AnimateText(Rigidbody _)
    {
        feedbackText.gameObject.SetActive(true);
        
        animSequence.Restart();
    }
}
