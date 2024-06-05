using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [Inject] private ScreenSaver screenSaver;
    [Inject] private Sounds sounds;
    [Inject] private Musics musics;

    [SerializeField] private RectTransform upperMainMenuPart;
    [SerializeField] private RectTransform lowerMainMenuPart;


    [SerializeField] private Button playB;
    

    // Start is called before the first frame update
    void Start()
    {        
        if (Globals.IsInitiated)
        {
            localize();
            playWhenInitialized();

        }

        playB.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundTypes.click);
            StartCoroutine(playStartLevel());
        });

        upperMainMenuPart.anchoredPosition = new Vector2(0, 500);
        lowerMainMenuPart.anchoredPosition = new Vector2(0, -500);
    }

    public void ShowSlowlyMainMenu(bool isScreenDelay)
    {
        float speedTimer = isScreenDelay ? 0.5f : 0.3f;

        upperMainMenuPart.anchoredPosition = new Vector2(0, 500);
        lowerMainMenuPart.anchoredPosition = new Vector2(0, -500);

        upperMainMenuPart.DOAnchorPos(Vector2.zero, speedTimer).SetEase(Ease.InSine);
        lowerMainMenuPart.DOAnchorPos(Vector2.zero, speedTimer).SetEase(Ease.InSine);
    }    

    private void resetData()
    {
        Globals.MainPlayerData = new PlayerData();
        SaveLoadManager.Save();
        Globals.CurrentLevel = 0;
        Globals.IsMainMenuTutorial = false;
        SceneManager.LoadScene("MainMenu");
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetData();
        }

        if (YandexGame.SDKEnabled && !Globals.IsInitiated)
        {
            Globals.IsInitiated = true;

            SaveLoadManager.Load();

            print("SDK enabled: " + YandexGame.SDKEnabled);
            Globals.CurrentLanguage = YandexGame.EnvironmentData.language;
            print("language set to: " + Globals.CurrentLanguage);

            Globals.IsMobile = Globals.IsMobileChecker();
            Globals.IsLowFPS = Globals.MainPlayerData.IsLowFPS;

            print("platform mobile: " + Globals.IsMobile);

            if (Globals.MainPlayerData.S == 1)
            {
                Globals.IsSoundOn = true;
                AudioListener.volume = 1;
            }
            else
            {
                Globals.IsSoundOn = false;
                AudioListener.volume = 0;
            }

            if (Globals.MainPlayerData.Mus == 1)
            {
                Globals.IsMusicOn = true;
            }
            else
            {
                Globals.IsMusicOn = false;
            }

            print("sound is: " + Globals.IsSoundOn);
            YandexGame.StickyAdActivity(!Globals.MainPlayerData.AdvOff);

            if (Globals.TimeWhenStartedPlaying == DateTime.MinValue)
            {
                Globals.TimeWhenStartedPlaying = DateTime.Now;
                Globals.TimeWhenLastInterstitialWas = DateTime.Now;
                Globals.TimeWhenLastRewardedWas = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            }

            localize();
            playWhenInitialized();
        }
    }


    private void playWhenInitialized()
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

        screenSaver.ShowScreen();
        ShowSlowlyMainMenu(true);
    }

    private IEnumerator playStartLevel()
    {
        screenSaver.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT * 1.5f);
        SceneManager.LoadScene("Gameplay");
    }

    private void localize()
    {
        if (Globals.Language == null)
        {
            Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        }

    }
}
