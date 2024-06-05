using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageDealer : MonoBehaviour
{    
    private SphereCollider mainCollider;

    private int myTeam;
    private int victimAmount;
    private Character myCharacter;
    private IPlayer myPlayer;
    private Action<Character> registerKilling;
    
    private bool isReturn;
    private bool isHide;

    private HashSet<IPlayer> victims = new HashSet<IPlayer>();

    private AssetManager assets;

    // Start is called before the first frame update
    void Awake()
    {
        mainCollider = GetComponent<SphereCollider>();
        mainCollider.isTrigger = true;
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
    }

    public void SetData(IPlayer myPl, int victimsAm, Action<Character> k, float radius)
    {
        SetData(myPl, victimsAm, k, radius, false, false);
    }

    public void SetData(IPlayer myPl, int victimsAm, Action<Character> k, float radius, bool isReturnToPool)
    {
        SetData(myPl, victimsAm, k, radius, isReturnToPool, false);
    }


    public void SetData(IPlayer myPl, int victimsAm, Action<Character> k, float radius, bool isReturnToPool, bool isHideAfterHit)
    {
        victims.Clear();
        mainCollider.enabled = true;


        isReturn = isReturnToPool;
        isHide = isHideAfterHit;
        mainCollider.radius = radius;
        myTeam = myPl.TeamID;        
        myPlayer = myPl;
        victimAmount = victimsAm;
        registerKilling = k;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player) && !victims.Contains(player) && player.TeamID != myTeam && victims.Count < victimAmount)
        {
            player.ReceiveHit(myPlayer, registerKilling);
            victims.Add(player);
            if (victims.Count >= victimAmount)
            {
                if (isReturn)
                {
                    assets.DamageDealerPool.ReturnObject(gameObject);
                }
                else
                {
                    if (isHide)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        mainCollider.enabled = false;
                    }
                    
                }
                
            }
                
        }
    }
}
