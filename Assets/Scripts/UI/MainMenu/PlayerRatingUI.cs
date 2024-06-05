using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRatingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Slider slider;

    private bool isStart;

    private static int[] rating = new int[]
    {
        0,
        1,
        100,
        500,
        1000,
        1500,
        2000,
        2700,
        3600
    };

    private void Update()
    {
        if (Globals.IsInitiated && !isStart)
        {
            isStart = true;
            UpdateRatingData();
        }
    }

    public void UpdateRatingData()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        levelText.text = GetCurrentLevel().ToString();
        progressText.text = $"{Globals.MainPlayerData.Rating}/{GetNextRatingValue()}";
        slider.value = (float)Globals.MainPlayerData.Rating / (float)GetNextRatingValue();
    }

    public static int GetCurrentLevel()
    {
        for (int i = 0; i < rating.Length; i++)
        {
            if (Globals.MainPlayerData.Rating < rating[i])
            {
                if (Globals.MainPlayerData.Rating <= 1)
                {
                    return 1;
                }
                else
                {
                    return (i - 1);
                }
            }            
        }

        return 11000;
    }

    public static int GetNextRatingValue()
    {
        for (int i = 0; i < rating.Length; i++)
        {
            if (Globals.MainPlayerData.Rating < rating[i])
            {
                return rating[i];
            }            
        }

        return 11000;
    }


}
