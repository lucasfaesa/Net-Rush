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
        animationFeedbackEventChannel.OnCutPowerChanged(PlayerActionsController.CutPowerEnum.None);
    }

    //called in animation event
    public void SetCutPowerWeak()
    {
        animationFeedbackEventChannel.OnCutPowerChanged(PlayerActionsController.CutPowerEnum.Weak);
    }
    
    //called in animation event
    public void SetCutPowerStrong()
    {
        animationFeedbackEventChannel.OnCutPowerChanged(PlayerActionsController.CutPowerEnum.Strong);
    }
    
    //called in animation event
    public void SetCutPowerVeryStrong()
    {
        animationFeedbackEventChannel.OnCutPowerChanged(PlayerActionsController.CutPowerEnum.VeryStrong);
    }
}
