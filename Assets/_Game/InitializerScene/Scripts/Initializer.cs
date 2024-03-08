using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{

    void Start()
    {
        #if !UNITY_WEBGL
            var resolutionWidth = PlayerPrefs.GetInt("resolutionWidth");
            var resolutionHeight = PlayerPrefs.GetInt("resolutionHeight");
            var refreshRate = PlayerPrefs.GetInt("refreshRate");
            var windowMode = PlayerPrefs.GetInt("windowMode");
            var vSyncCount = PlayerPrefs.GetInt("vSyncCount");
            var graphicsSettings = PlayerPrefs.GetString("graphicsSettings");

            if (resolutionWidth == 0)
            {
                resolutionWidth = 1280;
                PlayerPrefs.SetInt("resolutionWidth", resolutionWidth);
                
                resolutionHeight = 720;
                PlayerPrefs.SetInt("resolutionHeight", resolutionHeight);
                
                refreshRate = 0;
                PlayerPrefs.SetFloat("refreshRate", refreshRate);
                
                windowMode = (int)FullScreenMode.Windowed;
                PlayerPrefs.SetInt("windowMode", windowMode);
                
                vSyncCount = QualitySettings.vSyncCount;
                PlayerPrefs.SetInt("vSyncCount", vSyncCount);
                
                graphicsSettings = QualitySettings.names[QualitySettings.GetQualityLevel()];
                PlayerPrefs.SetString("graphicsSettings", graphicsSettings);
            }
        
            Screen.SetResolution(resolutionWidth, resolutionHeight, (FullScreenMode)windowMode, refreshRate);
            QualitySettings.vSyncCount = vSyncCount;
            QualitySettings.SetQualityLevel(new List<string>(QualitySettings.names).IndexOf(graphicsSettings), true);
                

            
        #endif

        SceneManager.LoadScene("MainMenuScene");
    }

}
