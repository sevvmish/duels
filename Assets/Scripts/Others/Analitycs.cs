using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Analitycs : MonoBehaviour
{
    public static Analitycs Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Send(string data)
    {
        //GP_Analytics.Goal(data, "");
        if (YandexGame.Instance != null) YandexMetrica.Send(data);
    }
}
