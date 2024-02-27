using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerEnterCheck : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [SerializeField] private PlayerStatsSO playerStats;
    [Space]
    [SerializeField] private ActionTypeEnum actionType;
    
    private enum ActionTypeEnum {CutFar, CutWide, CutNarrow, Bump}
    private bool _triggered;

    private Rigidbody ballRb;
    private BallBehavior _ballBehavior;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            _triggered = true;

            if (!ballRb)
            {
                other.TryGetComponent(out ballRb);
                other.TryGetComponent(out _ballBehavior);
            }
            
            _ballBehavior.LastPlayerToTouchBall(playerStats.PlayerSide);
            ballRb.isKinematic = false;
            
            switch (actionType)
            {
                case ActionTypeEnum.CutFar:
                    animationFeedbackEventChannel.OnCutFarTriggered(ballRb);
                    break;
                case ActionTypeEnum.CutWide:
                    animationFeedbackEventChannel.OnCutWideTriggered(ballRb);
                    break;
                case ActionTypeEnum.CutNarrow:
                    animationFeedbackEventChannel.OnCutNarrowTriggered(ballRb);
                    break;
                case ActionTypeEnum.Bump:
                    animationFeedbackEventChannel.OnBumpTriggered(ballRb);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        _triggered = false;
    }
}
