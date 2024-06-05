using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class GoldCoinsUI : MonoBehaviour
{
    [Inject] private Sounds sounds;

    [SerializeField] private TextMeshProUGUI text;

    private bool isStarted;
    private bool isEffectON;
    
    private void Update()
    {
        if (Globals.IsInitiated && !isStarted)
        {
            isStarted = true;
            UpdateGoldData(false);
        }
    }

    public void UpdateGoldData(bool isEffect)
    {
        int value = Globals.MainPlayerData.Gold;
        string data = "";
        if (value<1000)
        {
            data = value.ToString();
        }
        else if (value >=1000 && value < 1000000)
        {
            data = ((float)value / 1000).ToString("f1") + "K";
        }
        else
        {
            data = ((float)value / 1000000).ToString("f1") + "M";
        }

        text.text = data;

        if (isEffect && !isEffectON)
        {
            StartCoroutine(playEffect());
        }
    }

    private IEnumerator playEffect()
    {
        isEffectON = true;
        sounds.PlaySound(SoundTypes.coins);
        text.transform.DOShakeScale(0.3f, 1, 30).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.32f);
        isEffectON = false;
    }
}
