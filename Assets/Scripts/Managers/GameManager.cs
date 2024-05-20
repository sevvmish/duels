using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private ScreenSaver screenSaver;
    [Inject] private LevelManager levelManager;

    private void Awake()
    {
        if (Globals.IsMobile)
        {
            QualitySettings.antiAliasing = 2;

            if (Globals.IsLowFPS)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
            }

        }
        else
        {
            QualitySettings.antiAliasing = 4;
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }

        levelManager.SetLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenSaver.ShowScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
