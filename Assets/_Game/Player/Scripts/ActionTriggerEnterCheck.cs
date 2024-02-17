using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerEnterCheck : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [Space]
    [SerializeField] private ActionTypeEnum actionType;
    
    private enum ActionTypeEnum {CutWide, CutNarrow, Bump}
    private bool _triggered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            _triggered = true;
            
            switch (actionType)
            {
                case ActionTypeEnum.CutWide:
                    animationFeedbackEventChannel.OnCutWideTriggered(other.transform);
                    break;
                case ActionTypeEnum.CutNarrow:
                    animationFeedbackEventChannel.OnCutNarrowTriggered(other.transform);
                    break;
                case ActionTypeEnum.Bump:
                    animationFeedbackEventChannel.OnBumpTriggered(other.transform);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        _triggered = false;
    }
}
