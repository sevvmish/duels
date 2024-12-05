using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static bool IsSoundOn;
    public static bool IsMusicOn;
    public static bool IsInitiated;
    public static bool IsMainMenuTutorial;
    public static string CurrentLanguage;
    public static Translation Language;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public const float REWARDED_COOLDOWN = 70;
    public const float INTERSTITIAL_COOLDOWN = 63;

    public static bool IsMobile;
    public static bool IsOptions;
    public static bool IsLowFPS;

    public const float SCREEN_SAVER_AWAIT = 0.4f;
        
    public static MusicTypes LastPlayedMelody;

    public const float TIME_SPEED_KOEF = 500;
    public const float DISTANCE_FOR_USE = 4f;


    public const float BASE_SPEED = 7f;
    public const float JUMP_POWER = 40f;
    public const float GRAVITY_KOEFF = 2.75f;
    public const float SPEED_INC_IN_NONGROUND_PC = 0.25f;
    public const float SPEED_INC_IN_NONGROUND_MOBILE = 0.35f; 
    public const float MASS = 1.5f; //3
    public const float DRAG = 5f; //1
    public const float ANGULAR_DRAG = 5f;

    public const float ZOOM_DELTA = 0.5f;//0.3f;
    public const float ZOOM_LIMIT_LOW = 30f;
    public const float ZOOM_LIMIT_HIGH = 80f;

    public static readonly Vector3 BasePosition = new Vector3(0, 7f, -5f);
    public static readonly Vector3 BaseRotation = new Vector3(45, 0, 0);

    public const float MOUSE_X_SENS = 600f;
    public const float MOUSE_Y_SENS = 450f;

    public static float WORKING_DISTANCE = 20;
    public const float SHADOW_Y_DISTANCE = 8f;

    public static bool IsMobileChecker()
    {        
        //return true;

        if (Application.isMobilePlatform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ShowBanners()
    {
        if (!Globals.MainPlayerData.AdvOff && YandexGame.SDKEnabled)
        {
            YandexGame.StickyAdActivity(true);
        }
    }

    public static void SetQualityLevel()
    {
        if (Globals.IsMobile && !Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowDistance = 80;
            QualitySettings.shadows = ShadowQuality.HardOnly;
        }
        else if (Globals.IsMobile && Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 0;
            QualitySettings.shadows = ShadowQuality.Disable;
        }
        else if (!Globals.IsMobile && !Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 4;
            QualitySettings.shadowDistance = 100;
            QualitySettings.shadows = ShadowQuality.All;
        }
        else if (!Globals.IsMobile && Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowDistance = 60;
            QualitySettings.shadows = ShadowQuality.Disable;
        }
    }


}
