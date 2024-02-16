using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFeedback : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    
    //called in animation event
    public void FinishedBumpAnimation()
    {
        animationFeedbackEventChannel.OnBumpAnimationFinished();
    }

    //called in animation event
    public void FinishedCutAnimation()
    {
        animationFeedbackEventChannel.OnCutAnimationFinished();
    }
}
