using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameplayUI : MonoBehaviour
{
    [Inject] private GameManager gm;
    [Inject] private Sounds sounds;
    [Inject] private MainPlayerControl mainPlayerControl;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI goldText;

    private float _timerForTimer = 1;
    private int mainPlayerCoins;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (_timerForTimer > 1 && gm.SecondsLeftToEndGame >= 0)
        {
            _timerForTimer = 0;
            showTimerData(gm.SecondsLeftToEndGame);
        }
        else
        {
            _timerForTimer += Time.deltaTime;
        }

        //coins update
        if (mainPlayerCoins != mainPlayerControl.MainDomain.GoldCoins)
        {
            mainPlayerCoins = mainPlayerControl.MainDomain.GoldCoins;
            goldText.text = mainPlayerCoins.ToString("f0");
        }
    }

    public void showTimerData(float _time)
    {        
        if (_time < 0)
        {
            _time = 0;
        }
            

        int newTime = Mathf.RoundToInt(_time);
        string result = "";

        if (newTime < 60)
        {
            result = newTime.ToString();
        }
        else
        {
            int m = newTime / 60;
            int s = newTime % 60;

            string min = m < 10 ? "0" + m.ToString() : m.ToString();
            string sec = s < 10 ? "0" + s.ToString() : s.ToString();

            result = min + ":" + sec.ToString();
        }

        timerText.text = result;

        if (newTime <= 5)
        {
            timerText.color = Color.red;
            sounds.PlaySound(SoundTypes.beepTick);
        }
        else if (newTime <= 10)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

}
