using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private BallEventsChannelSO ballEventsChannel;
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [Header("Triggers")]
    [SerializeField] private ActionTriggerEnterCheck cutFarTrigger;
    [SerializeField] private ActionTriggerEnterCheck cutWideTrigger;
    [SerializeField] private ActionTriggerEnterCheck cutNarrowTrigger;
    [SerializeField] private ActionTriggerEnterCheck bumpTrigger;
    [Header("Bracelet")] 
    [SerializeField] private List<Renderer> bracelets;
    [Header("Audio")] 
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private AudioClipSO cutNarrowAudio;
    [SerializeField] private AudioClipSO cutWideAudio;
    [SerializeField] private AudioClipSO cutFarAudio;
    [SerializeField] private AudioClipSO bumpAudio;
    [SerializeField] private AudioClipSO ballImpactOnHandsAudio;
    
    public enum CutPowerEnum { None, Weak, Strong, VeryStrong };
    private CutPowerEnum CurrentCutPower { get; set; }
    
    private bool _executingAction;

    private int _executedActions;
    
    private static readonly int CuttingFar = Animator.StringToHash("CuttingFar");
    private static readonly int CuttingWide = Animator.StringToHash("CuttingWide");
    private static readonly int CuttingNarrow = Animator.StringToHash("CuttingNarrow");
    private static readonly int Bumping = Animator.StringToHash("Bumping");
    
    private void OnEnable()
    {
        gameEventsChannel.GameEnded += KillPlayerActions;
        ballEventsChannel.BallIn += ResetActionsExecuted;
        ballEventsChannel.BallOut += ResetActionsExecuted;
        inputReader.CutNarrow += CutNarrowPlayerInput;
        inputReader.CutWide += CutWidePlayerInput;
        inputReader.CutFar += CutFarPlayerInput;
        inputReader.Bump += BumpPlayerInput;
        animationFeedbackEventChannel.CutAnimationFinished += CutAnimationEnded;
        animationFeedbackEventChannel.BumpAnimationFinished += BumpAnimationEnded;
        animationFeedbackEventChannel.CutPowerChanged += UpdateCutPower;
        animationFeedbackEventChannel.CutFarTriggered += OnCutFarHitBall;
        animationFeedbackEventChannel.CutWideTriggered += OnCutWideHitBall;
        animationFeedbackEventChannel.CutNarrowTriggered += OnCutNarrowHitBall;
        animationFeedbackEventChannel.BumpTriggered += OnBumpHitBall;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameEnded -= KillPlayerActions;
        ballEventsChannel.BallIn -= ResetActionsExecuted;
        ballEventsChannel.BallOut -= ResetActionsExecuted;
        inputReader.CutNarrow -= CutNarrowPlayerInput;
        inputReader.CutWide -= CutWidePlayerInput;
        inputReader.CutFar -= CutFarPlayerInput;
        inputReader.Bump -= BumpPlayerInput;
        animationFeedbackEventChannel.CutAnimationFinished -= CutAnimationEnded;
        animationFeedbackEventChannel.BumpAnimationFinished -= BumpAnimationEnded;
        animationFeedbackEventChannel.CutPowerChanged -= UpdateCutPower;
        animationFeedbackEventChannel.CutWideTriggered -= OnCutWideHitBall;
        animationFeedbackEventChannel.CutFarTriggered -= OnCutFarHitBall;
        animationFeedbackEventChannel.CutNarrowTriggered -= OnCutNarrowHitBall;
        animationFeedbackEventChannel.BumpTriggered -= OnBumpHitBall;
    }

    private void KillPlayerActions()
    {
        this.enabled = false;
    }


    private void CutNarrowPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        audioPlayer.PlaySFX(cutNarrowAudio);
        animator.SetBool(CuttingNarrow, true);
        cutNarrowTrigger.gameObject.SetActive(true);
        _executingAction = true;
    }
    
    private void CutWidePlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        audioPlayer.PlaySFX(cutWideAudio);
        animator.SetBool(CuttingWide, true);
        cutWideTrigger.gameObject.SetActive(true);
        _executingAction = true;
    }
    
    private void CutFarPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        audioPlayer.PlaySFX(cutFarAudio);
        animator.SetBool(CuttingFar, true);
        cutFarTrigger.gameObject.SetActive(true);
        _executingAction = true;
    }

    private void BumpPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        audioPlayer.PlaySFX(bumpAudio);
        animator.SetBool(Bumping, true);
        bumpTrigger.gameObject.SetActive(true);
        _executingAction = true;
    }
    
    private void CutAnimationEnded()
    {
        cutWideTrigger.gameObject.SetActive(false);
        cutNarrowTrigger.gameObject.SetActive(false);
        cutFarTrigger.gameObject.SetActive(false);
        animator.SetBool(CuttingWide, false);
        animator.SetBool(CuttingNarrow, false);
        animator.SetBool(CuttingFar, false);
        _executingAction = false;
    }

    private void BumpAnimationEnded()
    {
        bumpTrigger.gameObject.SetActive(false);
        animator.SetBool(Bumping, false);
        _executingAction = false;
    }

    private void UpdateCutPower(CutPowerEnum cutPowerEnum)
    {
        CurrentCutPower = cutPowerEnum;
    }

    private void CountActionExecuted()
    {
        _executedActions += 1;
        bracelets[_executedActions - 1].material.color = Color.red;
    }

    private void ResetActionsExecuted(PlayerStatsSO.PlayerSideEnum _, BallEventsChannelSO.FieldSideEnum __, bool ___)
    {
        _executedActions = 0;
        bracelets.ForEach(x=>x.material.color = Color.green);
    }
    
    private void ResetActionsExecuted(PlayerStatsSO.PlayerSideEnum _)
    {
        _executedActions = 0;
        bracelets.ForEach(x=>x.material.color = Color.green);
    }
    
    private void OnCutNarrowHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, playerStats.NarrowCutAngle, playerStats.WeakForceNarrowCut, playerStats.StrongForceNarrowCut, playerStats.VeryStrongForceNarrowCut);
    }

    private void OnCutWideHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, playerStats.WideCutAngle, playerStats.WeakForceWideCut, playerStats.StrongForceWideCut, playerStats.VeryStrongForceWideCut);
    }

    private void OnCutFarHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, playerStats.FarCutAngle, playerStats.WeakForceFarCut, playerStats.StrongForceFarCut, playerStats.VeryStrongForceFarCut);
    }
    
    private void ApplyCutForce(Rigidbody ballRb, float cutAngle, float weakForce, float strongForce, float veryStrongForce)
    {
        if (_executedActions >= playerStats.MaxNumberOfActions)
            return;
        
        audioPlayer.PlaySFX(ballImpactOnHandsAudio);
        
        CountActionExecuted();
        
        if(playerStats.PlayerSide == PlayerStatsSO.PlayerSideEnum.LeftSide)
            ballRb.AddTorque(new Vector3(1f,0f,0f) * -10f, ForceMode.VelocityChange);
        if(playerStats.PlayerSide == PlayerStatsSO.PlayerSideEnum.RightSide)
            ballRb.AddTorque(new Vector3(1f,0f,0f) * 10f, ForceMode.VelocityChange);
        
        // Calcula a rotação em torno do eixo X com o offset desejado
        Quaternion rotation = Quaternion.Euler(cutAngle, 0f, 0f);

        // Rotaciona a força para frente do transform com o offset
        Vector3 rotatedForce = rotation * transform.forward;

        float forceMultiplier = 1f;

        switch (CurrentCutPower)
        {
            case CutPowerEnum.Weak:
                forceMultiplier = weakForce;
                break;
            case CutPowerEnum.Strong:
                forceMultiplier = strongForce;
                break;
            case CutPowerEnum.VeryStrong:
                forceMultiplier = veryStrongForce;
                break;
            
            default:
                break;
        }

        ballRb.AddForce(rotatedForce * forceMultiplier, ForceMode.VelocityChange);
    }
    
    private void OnBumpHitBall(Rigidbody ballRb)
    {
        if (_executedActions >= playerStats.MaxNumberOfActions)
            return;
        
        audioPlayer.PlaySFX(ballImpactOnHandsAudio);
        
        CountActionExecuted();
        
        float result;
        if (inputReader.Direction().x > 0)
        {
            result = -playerStats.BumpForceOnMovement;
            
            //rotates the ball visually only
            ballRb.AddTorque(new Vector3(1f,0f,0f) * -10f, ForceMode.VelocityChange);
        }
        else if (inputReader.Direction().x < 0)
        {
            result = playerStats.BumpForceOnMovement;
            
            //rotates the ball visually only
            ballRb.AddTorque(new Vector3(1f,0f,0f) * 10f, ForceMode.VelocityChange);
        }
        else
            result = 0;
        
        //makes the ball goes slightly to the direction the player is moving
        Quaternion rotation = Quaternion.Euler(result, 0f, 0f);
        Vector3 rotatedForce = rotation * transform.up;

        ballRb.AddForce(rotatedForce * playerStats.BumpForce, ForceMode.VelocityChange);
    }

    
}
