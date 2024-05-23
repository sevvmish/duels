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

    public static int CurrentLevel;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public const float REWARDED_COOLDOWN = 120;
    public const float INTERSTITIAL_COOLDOWN = 70;

    public static bool IsMobile;
    public static bool IsOptions;
    public static bool IsLowFPS;

    public const float SCREEN_SAVER_AWAIT = 0.75f;

    public const float PLAYER_UPDATE_JOYSTICK_COOLDOWN = 0.1f;
    public const float PLAYER_BASE_MAXSPEED = 7f;


    public static bool IsMobileChecker()
    {
        if (Application.isMobilePlatform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
