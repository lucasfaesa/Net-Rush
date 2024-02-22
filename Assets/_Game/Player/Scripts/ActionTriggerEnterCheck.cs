using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerEnterCheck : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    
    [SerializeField] private ActionTypeEnum actionType;
    
    private enum ActionTypeEnum {CutFar, CutWide, CutNarrow, Bump}
    private bool _triggered;

    private Rigidbody ballRb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            _triggered = true;
            
            //TODO cache the ball on a variable later, because will be the same ball everytime
            other.TryGetComponent(out ballRb);
            
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
