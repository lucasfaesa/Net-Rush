using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private AnimationFeedbackEventChannelSO animationFeedbackEventChannel;
    [SerializeField] private InputReader inputReader;
    [Space]
    [SerializeField] private Animator animator;
    [Header("Triggers")] 
    [SerializeField] private GameObject cutWideTrigger;
    [SerializeField] private GameObject cutNarrowTrigger;
    [SerializeField] private GameObject bumpTrigger;
    [Header("Debug")] 
    [SerializeField] private float weakForce = 5;
    [SerializeField] private float strongForce = 10;
    [SerializeField] private float VeryStrongForce = 15;
    [Space] 
    [SerializeField] private float wideCutAngle = 60;
    [SerializeField] private float narrowCutAngle = 15;
    
    public enum CutPowerEnum { None, Weak, Strong, VeryStrong };

    private CutPowerEnum CurrentCutPower { get; set; }
    
    private bool _executingAction;
    
    private static readonly int CuttingWide = Animator.StringToHash("CuttingWide");
    private static readonly int CuttingNarrow = Animator.StringToHash("CuttingNarrow");
    private static readonly int Bumping = Animator.StringToHash("Bumping");
    
    private void OnEnable()
    {
        inputReader.CutWide += CutWidePlayerInput;
        inputReader.CutNarrow += CutNarrowPlayerInput;
        inputReader.Bump += BumpPlayerInput;
        animationFeedbackEventChannel.CutAnimationFinished += CutAnimationEnded;
        animationFeedbackEventChannel.BumpAnimationFinished += BumpAnimationEnded;
        animationFeedbackEventChannel.CutPowerChanged += UpdateCutPower;
        animationFeedbackEventChannel.CutWideTriggered += OnCutWideHitBall;
        animationFeedbackEventChannel.CutNarrowTriggered += OnCutNarrowHitBall;
        animationFeedbackEventChannel.BumpTriggered += OnBumpHitBall;
    }

    private void OnDisable()
    {
        inputReader.CutWide -= CutWidePlayerInput;
        inputReader.CutNarrow -= CutNarrowPlayerInput;
        inputReader.Bump -= BumpPlayerInput;
        animationFeedbackEventChannel.CutAnimationFinished -= CutAnimationEnded;
        animationFeedbackEventChannel.BumpAnimationFinished -= BumpAnimationEnded;
        animationFeedbackEventChannel.CutPowerChanged -= UpdateCutPower;
        animationFeedbackEventChannel.CutWideTriggered -= OnCutWideHitBall;
        animationFeedbackEventChannel.CutNarrowTriggered -= OnCutNarrowHitBall;
        animationFeedbackEventChannel.BumpTriggered -= OnBumpHitBall;
    }

    private void CutWidePlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(CuttingWide, true);
        cutWideTrigger.SetActive(true);
        _executingAction = true;
    }
    
    private void CutNarrowPlayerInput(bool pressed)
    {
        if (_executingAction || !pressed) return;
        
        animator.SetBool(CuttingNarrow, true);
        cutNarrowTrigger.SetActive(true);
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
        animator.SetBool(CuttingWide, false);
        animator.SetBool(CuttingNarrow, false);
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

    private void OnCutWideHitBall(Transform ballTransform)
    {
        //TODO ARMAZENAR A REFERENCIA DA BOLA, PRA NAO FICAR DANDO GETCOMPONENT
        
        if(ballTransform.TryGetComponent<Rigidbody>(out var ballRb))
        {
            // Calcula a rotação em torno do eixo X com o offset desejado
            Quaternion rotation = Quaternion.Euler(wideCutAngle, 0f, 0f);

            // Rotaciona a força para frente do transform com o offset
            Vector3 rotatedForce = rotation * transform.forward;
            
            switch (CurrentCutPower)
            {
                case CutPowerEnum.None:
                        ballRb.AddForce(rotatedForce * 1, ForceMode.VelocityChange);
                    break;
                case CutPowerEnum.Weak:
                        ballRb.AddForce(rotatedForce * weakForce, ForceMode.VelocityChange);
                    break;
                case CutPowerEnum.Strong:
                        ballRb.AddForce(rotatedForce * strongForce, ForceMode.VelocityChange);  
                    break;
                case CutPowerEnum.VeryStrong:
                        ballRb.AddForce(rotatedForce * VeryStrongForce, ForceMode.VelocityChange);
                    break;
            }
            
            Debug.Log($"Current cut Wide power: {CurrentCutPower}");
        }
        
        
    }
    
    private void OnCutNarrowHitBall(Transform ballTransform)
    {
        //TODO ARMAZENAR A REFERENCIA DA BOLA, PRA NAO FICAR DANDO GETCOMPONENT
        
        if(ballTransform.TryGetComponent<Rigidbody>(out var ballRb))
        {
            // Calcula a rotação em torno do eixo X com o offset desejado
            Quaternion rotation = Quaternion.Euler(narrowCutAngle, 0f, 0f);

            // Rotaciona a força para frente do transform com o offset
            Vector3 rotatedForce = rotation * transform.forward;
            
            switch (CurrentCutPower)
            {
                case CutPowerEnum.None:
                    ballRb.AddForce(rotatedForce * 1, ForceMode.VelocityChange);
                    break;
                case CutPowerEnum.Weak:
                    ballRb.AddForce(rotatedForce * weakForce, ForceMode.VelocityChange);
                    break;
                case CutPowerEnum.Strong:
                    ballRb.AddForce(rotatedForce * strongForce, ForceMode.VelocityChange);  
                    break;
                case CutPowerEnum.VeryStrong:
                    ballRb.AddForce(rotatedForce * VeryStrongForce, ForceMode.VelocityChange);
                    break;
            }
            
            Debug.Log($"Current cut Narrow power: {CurrentCutPower}");
        }
        
    }

    private void OnBumpHitBall(Transform ballTransform)
    {
        //TODO ARMAZENAR A REFERENCIA DA BOLA, PRA NAO FICAR DANDO GETCOMPONENT
        
        if (ballTransform.TryGetComponent<Rigidbody>(out var ballRb))
        {
            ballRb.AddForce(this.transform.up * strongForce, ForceMode.VelocityChange);    
        }
    }
}
