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

    private HashSet<IPlayer> victims = new HashSet<IPlayer>();

    private AssetManager assets;

    // Start is called before the first frame update
    void Awake()
    {
        mainCollider = GetComponent<SphereCollider>();
        mainCollider.isTrigger = true;
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
    }
        
    public void SetData(IPlayer myPl, int victimsAm, Action<Character> k, float radius, bool isReturnToPool)
    {
        victims.Clear();
        mainCollider.enabled = true;


        isReturn = isReturnToPool;
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
                    mainCollider.enabled = false;
                }
                
            }
                
        }
    }
}
