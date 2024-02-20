using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [SerializeField] private InputReader inputReader;
    [Space]
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [Header("Triggers")]
    [SerializeField] private GameObject cutFarTrigger;
    [SerializeField] private GameObject cutWideTrigger;
    [SerializeField] private GameObject cutNarrowTrigger;
    [SerializeField] private GameObject bumpTrigger;
    [Header("Actions Settings")]
    [Header("Narrow Cut Settings")]
    [SerializeField] private float narrowCutAngle = 35f;
    [SerializeField] private float weakForceNarrowCut = 5f;
    [SerializeField] private float strongForceNarrowCut = 10f;
    [SerializeField] private float veryStrongForceNarrowCut = 15f;
    [Header("Wide Cut Settings")]
    [SerializeField] private float wideCutAngle = 8f;
    [SerializeField] private float weakForceWideCut = 5f;
    [SerializeField] private float strongForceWideCut = 10f;
    [SerializeField] private float veryStrongForceWideCut = 15f;
    [Header("Far Cut Settings")]
    [SerializeField] private float farCutAngle = -15f;
    [SerializeField] private float weakForceFarCut = 9f;
    [SerializeField] private float strongForceFarCut = 12f;
    [SerializeField] private float VeryStrongForceFarCut = 13.5f;
    [Header("Bump Settings")]
    [SerializeField] private float bumpForce = 10;
    [SerializeField] private float bumpForceOnMovement = 8;
    
    public enum CutPowerEnum { None, Weak, Strong, VeryStrong };
    private CutPowerEnum CurrentCutPower { get; set; }
    
    private bool _executingAction;
    
    private static readonly int CuttingFar = Animator.StringToHash("CuttingFar");
    private static readonly int CuttingWide = Animator.StringToHash("CuttingWide");
    private static readonly int CuttingNarrow = Animator.StringToHash("CuttingNarrow");
    private static readonly int Bumping = Animator.StringToHash("Bumping");
    
    private void OnEnable()
    {
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

    private void CutNarrowPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(CuttingNarrow, true);
        cutNarrowTrigger.SetActive(true);
        _executingAction = true;
    }
    
    private void CutWidePlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(CuttingWide, true);
        cutWideTrigger.SetActive(true);
        _executingAction = true;
    }
    
    private void CutFarPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(CuttingFar, true);
        cutFarTrigger.SetActive(true);
        _executingAction = true;
    }

    private void BumpPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(Bumping, true);
        bumpTrigger.SetActive(true);
        _executingAction = true;
    }
    
    private void CutAnimationEnded()
    {
        cutWideTrigger.SetActive(false);
        cutNarrowTrigger.SetActive(false);
        cutFarTrigger.SetActive(false);
        animator.SetBool(CuttingWide, false);
        animator.SetBool(CuttingNarrow, false);
        animator.SetBool(CuttingFar, false);
        _executingAction = false;
    }

    private void BumpAnimationEnded()
    {
        bumpTrigger.SetActive(false);
        animator.SetBool(Bumping, false);
        _executingAction = false;
    }

    private void UpdateCutPower(CutPowerEnum cutPowerEnum)
    {
        CurrentCutPower = cutPowerEnum;
    }

    private void OnCutNarrowHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, narrowCutAngle, weakForceNarrowCut, strongForceNarrowCut, veryStrongForceNarrowCut);
        Debug.Log($"Current cut Narrow power: {CurrentCutPower}");
    }

    private void OnCutWideHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, wideCutAngle, weakForceWideCut, strongForceWideCut, veryStrongForceWideCut);
        Debug.Log($"Current cut Wide power: {CurrentCutPower}");
    }

    private void OnCutFarHitBall(Rigidbody ballRb)
    {
        ApplyCutForce(ballRb, farCutAngle, weakForceFarCut, strongForceFarCut, VeryStrongForceFarCut);
        Debug.Log($"Current cut Far power: {CurrentCutPower}");
    }
    
    private void ApplyCutForce(Rigidbody ballRb, float cutAngle, float weakForce, float strongForce, float veryStrongForce)
    {
        //resetting ball velocity
        ballRb.velocity = Vector3.zero;
        
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
        //resetting ball velocity
        ballRb.velocity = Vector3.zero;
        
        float result;
        if (inputReader.Direction.x > 0)
            result = -bumpForceOnMovement;
        else if (inputReader.Direction.x < 0)
            result = bumpForceOnMovement;
        else
            result = 0;
        
        //makes the ball goes slightly to the direction the player is moving
        Quaternion rotation = Quaternion.Euler(result, 0f, 0f);
        Vector3 rotatedForce = rotation * transform.up;

        ballRb.AddForce(rotatedForce * bumpForce, ForceMode.VelocityChange);
    }
}
