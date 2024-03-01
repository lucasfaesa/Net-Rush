using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Transform SpritesFader;
    [SerializeField] private List<Transform> stripes = new();
    
    private void Awake()
    {
        FadeOut();
    }

    public void FadeOut()
    {
        SpritesFader.gameObject.SetActive(true);
        stripes.ForEach(x=>x.gameObject.SetActive(true));
        
        DOTween.Sequence()
            .Append(stripes[0].DOLocalMoveX(-10000f, 0.3f).SetEase(Ease.InBack, 1))
            .Insert(0f, stripes[1].DOLocalMoveX(-10000f, 0.4f).SetEase(Ease.InBack,1))
            .Insert(0f, stripes[2].DOLocalMoveX(10000f, 0.5f).SetEase(Ease.InBack, 1))
            .OnComplete(() =>
            {
                SpritesFader.gameObject.SetActive(false);
                SetObjectsOutOfScreen();
                stripes.ForEach(x=>x.gameObject.SetActive(false));
            });
    }
    

    public void FadeIn(Action onCompleteCallback)
    {
        SetObjectsOutOfScreen();
        SpritesFader.gameObject.SetActive(true);
        stripes.ForEach(x=>x.gameObject.SetActive(true));
        DOTween.Sequence().Append(stripes[0].DOLocalMoveX(-624f, 0.5f).SetEase(Ease.OutSine, 1))
            .Insert(0f, stripes[1].DOLocalMoveX(453f, 0.6f).SetEase(Ease.OutSine, 1))
            .Insert(0f, stripes[2].DOLocalMoveX(1190f, 0.7f).SetEase(Ease.OutSine, 1))
            .OnComplete(() =>
            {
                onCompleteCallback?.Invoke();
            });
    }
    
    private void SetObjectsOutOfScreen()
    {
        stripes[0].transform.localPosition = new Vector3(-10000f, 0f, 0f);
        stripes[1].transform.localPosition = new Vector3(-10000f, 0f, 0f);
        stripes[2].transform.localPosition = new Vector3(10000f, 0f, 0f);
        
        stripes.ForEach(x=>x.gameObject.SetActive(true));
    }
}
