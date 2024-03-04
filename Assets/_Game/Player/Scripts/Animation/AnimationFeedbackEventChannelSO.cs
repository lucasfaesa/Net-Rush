using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationFeedbackEventChannel", menuName = "ScriptableObjects/Player/AnimationFeedbackEventChannel")]
public class AnimationFeedbackEventChannelSO : ScriptableObject
{
    public event Action BumpAnimationFinished;
    public event Action CutAnimationFinished;
    public event Action<Rigidbody> CutFarTriggered;
    public event Action<Rigidbody> CutWideTriggered;
    public event Action<Rigidbody> CutNarrowTriggered;
    public event Action<Rigidbody> BumpTriggered;
    public event Action<PlayerActionsController.CutPowerEnum> CutPowerChanged;
    public event Action PlayerFootHitGround;
    
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
    
    public void OnCutFarTriggered(Rigidbody ballRb)
    {
        CutFarTriggered?.Invoke(ballRb);
    }
    
    public void OnCutWideTriggered(Rigidbody ballRb)
    {
        CutWideTriggered?.Invoke(ballRb);
    }
    
    public void OnCutNarrowTriggered(Rigidbody ballRb)
    {
        CutNarrowTriggered?.Invoke(ballRb);
    }
    
    public void OnBumpTriggered(Rigidbody ballRb)
    {
        BumpTriggered?.Invoke(ballRb);
    }

    public void OnPlayerFootHitGround()
    {
        PlayerFootHitGround?.Invoke();
        Debug.Log("Step Sound");
    }
}
