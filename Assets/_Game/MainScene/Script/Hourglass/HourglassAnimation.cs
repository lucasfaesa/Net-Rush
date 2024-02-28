using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HourglassAnimation : MonoBehaviour
{

    [SerializeField] private float targetYPos = 4.79f;
    [SerializeField] private float duration = 2f;
    
    void Start()
    {
        this.transform.DOMoveY(targetYPos, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
