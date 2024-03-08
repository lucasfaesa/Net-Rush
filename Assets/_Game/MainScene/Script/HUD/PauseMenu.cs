using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private Fader fader;
    [Space]
    [SerializeField] private Transform pauseMenuWindow;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button firstSelected;

    private bool _pauseMenuShown;
    
    public void TogglePauseMenu(bool show)
    {
        _pauseMenuShown = show;
        
        pauseMenuWindow.gameObject.SetActive(show);
        
        if(show)
            firstSelected.Select();
        
        Time.timeScale = show ? 0f : 1f;
    }
    
    public void ReloadScene()
    {
        canvasGroup.interactable = false;

        Time.timeScale = 1f;
        
        pauseMenuWindow.gameObject.SetActive(false);
        
        fader.FadeIn(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        });
    }
    
    public void LoadMainMenuScene()
    {
        canvasGroup.interactable = false;
        
        Time.timeScale = 1f;
        
        pauseMenuWindow.gameObject.SetActive(false);
        
        fader.FadeIn(() =>
        {
            SceneManager.LoadScene("MainMenuScene");    
        });
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            _pauseMenuShown = !_pauseMenuShown;
            TogglePauseMenu(_pauseMenuShown);
        }
            
    }
}
