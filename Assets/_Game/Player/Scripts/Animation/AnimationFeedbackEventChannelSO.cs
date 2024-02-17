using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationFeedbackEventChannel", menuName = "ScriptableObjects/Player/AnimationFeedbackEventChannel")]
public class AnimationFeedbackEventChannelSO : ScriptableObject
{
    public event Action BumpAnimationFinished;
    public event Action CutAnimationFinished;
    public event Action<Transform> CutWideTriggered;
    public event Action<Transform> CutNarrowTriggered;
    public event Action<Transform> BumpTriggered;
    public event Action<PlayerActionsController.CutPowerEnum> CutPowerChanged;
    
    public void OnBumpAnimationFinished()
    {
        BumpAnimationFinished?.Invoke();
    }

    public void OnCutAnimationFinished()
    {
        CutAnimationFinished?.Invoke();
    }

    public void OnCutPowerChanged(PlayerActionsController.CutPowerEnum power)
    {
        CutPowerChanged?.Invoke(power);
    }
    
    public void OnCutWideTriggered(Transform ballTransform)
    {
        CutWideTriggered?.Invoke(ballTransform);
    }
    
    public void OnCutNarrowTriggered(Transform ballTransform)
    {
        CutNarrowTriggered?.Invoke(ballTransform);
    }
    
    public void OnBumpTriggered(Transform ballTransform)
    {
        BumpTriggered?.Invoke(ballTransform);
    }
    
}
