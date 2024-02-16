using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationFeedbackEventChannel", menuName = "ScriptableObjects/Player/AnimationFeedbackEventChannel")]
public class AnimationFeedbackEventChannelSO : ScriptableObject
{
    public event Action BumpAnimationFinished;
    public event Action CutAnimationFinished;

    public void OnBumpAnimationFinished()
    {
        BumpAnimationFinished?.Invoke();
    }

    public void OnCutAnimationFinished()
    {
        CutAnimationFinished?.Invoke();
    }
}
