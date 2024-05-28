using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject forPlayers;
    [SerializeField] private Image forPlayersBackIndicator;
    [SerializeField] private Image forPlayersIndicator;

    [SerializeField] private GameObject forNPCs;
    [SerializeField] private Image forNPCsBackIndicator;
    [SerializeField] private Image forNPCsIndicator;

    public RectTransform Rect;

    private bool isPlayer;
    private IPlayer characterManager;
    private Action<IPlayer> remover;

    private float currentValue;

    public void SetData(bool isPlayer, IPlayer c, Action<IPlayer> remover)
    {
        this.remover = remover;
        this.isPlayer = isPlayer;
        characterManager = c;
        Rect = GetComponent<RectTransform>();

        if (isPlayer)
        {
            forPlayers.SetActive(true);
            forNPCs.SetActive(false);
            forPlayersBackIndicator.fillAmount = 1;
            forPlayersIndicator.fillAmount = 1;
        }
        else 
        {
            forPlayers.SetActive(false);
            forNPCs.SetActive(true);
            forNPCsBackIndicator.fillAmount = 1;
            forNPCsIndicator.fillAmount = 1;
        }
    }

    public void UpdateHP()
    {
        float newValue = characterManager.Character.CurrentHP / characterManager.Character.MaxHP;
        if (currentValue != newValue)
        {
            currentValue = newValue;
        }
        else
        {
            return;
        }

        if (isPlayer)
        {            
            if (newValue > 0)
            {
                if (Globals.IsLowFPS)
                {                    
                    forPlayersBackIndicator.fillAmount = newValue;
                    forPlayersIndicator.fillAmount = newValue;
                }
                else
                {
                    forPlayersBackIndicator.DOKill();
                    forPlayersBackIndicator.DOFillAmount(newValue, 0.5f);
                    forPlayersIndicator.fillAmount = newValue;
                }
                
            }
            else
            {
                forPlayersBackIndicator.DOKill();
                forPlayersBackIndicator.fillAmount = 0;
                forPlayersIndicator.fillAmount = 0;
                remover(characterManager);
            }

            
        }
        else
        {            
            if (newValue > 0)
            {
                if (Globals.IsLowFPS)
                {
                    forNPCsBackIndicator.fillAmount = newValue;
                    forNPCsIndicator.fillAmount = newValue;
                }
                else
                {
                    forNPCsBackIndicator.DOKill();
                    forNPCsBackIndicator.DOFillAmount(newValue, 0.5f);
                    forNPCsIndicator.fillAmount = newValue;
                }

                
            }
            else
            {
                forNPCsBackIndicator.DOKill();
                forNPCsBackIndicator.fillAmount= 0;
                forNPCsIndicator.fillAmount = 0;
                remover(characterManager);
            }            
        }
    }

}
