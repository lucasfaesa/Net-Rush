using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private Fader fader;
    [Space]
    [SerializeField] private Button firstSelected;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("How To Play")] 
    [SerializeField] private Transform howToPlayWindow;
    [SerializeField] private Button exitHowToPlayButton;
    [Header("Credits")]
    [SerializeField] private Transform creditsWindow;
    [SerializeField] private Button exitCreditsWindow;
    
    private void Start()
    {
        firstSelected.Select();
    }

    public void LoadGameScene()
    {
        fader.FadeIn(() =>
        {
            canvasGroup.interactable = false;
            SceneManager.LoadScene("MainScene");
        });
    }

    public void ToggleHowToPlayWindow(bool close)
    {
        howToPlayWindow.gameObject.SetActive(!close);
        
        if (close)
            firstSelected.Select();
        else
            exitHowToPlayButton.Select();
    }
    
    public void ToggleCreditsWindow(bool close)
    {
        creditsWindow.gameObject.SetActive(!close);
        
        if (close)
            firstSelected.Select();
        else
            exitCreditsWindow.Select();
    }
    
    public void CloseGame()
    {
        Application.Quit();
    }
}
