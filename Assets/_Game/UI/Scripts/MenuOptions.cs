using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private HudEventsChannelSO hudEventsChannel;
    [Space]
    [SerializeField] private Button buttonToStartSelected;
    [SerializeField] private CanvasGroup canvasGroup;
    [Space] 
    [SerializeField] private Fader fader;
    
    private void OnEnable()
    {
        hudEventsChannel.GameWonHudAnimationEnded += ShowMenuOptions;
    }

    private void OnDisable()
    {
        hudEventsChannel.GameWonHudAnimationEnded -= ShowMenuOptions;
    }

    private void ShowMenuOptions()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 1f).SetEase(Ease.Linear);
    }
    
    void Start()
    {
        buttonToStartSelected.Select();    
    }

    public void ReloadScene()
    {
        fader.FadeIn(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        });
    }

    public void LoadMainMenu()
    {
        //SceneManager.LoadScene()
    }
}
