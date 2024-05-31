using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAttack : MonoBehaviour
{    
    private AssetManager assets;
    private EffectsManager effects;
    private Character character;
    private AnimationControl _animator;
    private IPlayer myPlayer;
    private float attackSpeed => character.AttackSpeed;
    private float currentDamage => character.CurrentDamage;

    private float _timer;
    private bool isReadyToHit => _timer >= attackSpeed;

    private Action<Character> registerKilling;

    private void Start()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        effects = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
    }

    public void SetData(IPlayer characterM, AnimationControl a, Action<Character> k)
    {
        myPlayer = characterM;
        character = characterM.Character;
        _animator = a;
        _timer = attackSpeed + 0.1f;
        registerKilling = k;
    }

    private void Update()
    {        
        if (character != null && _timer >= attackSpeed)
        {            
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void Hit(IPlayer aim)
    {        
        if (isReadyToHit)
        {
            _timer = 0;

            switch (character.CharacterTypeByUniqueName)
            {
                case CharacterTypesByUniqueName.WarriorSam:
                    StartCoroutine(meleeHit(aim));
                    break;

                case CharacterTypesByUniqueName.ShooterMike:
                    StartCoroutine(bowHit(aim));
                    break;

                case CharacterTypesByUniqueName.TestBoss:
                    StartCoroutine(meleeHit(aim));
                    break;

                case CharacterTypesByUniqueName.VikingHero:
                    StartCoroutine(meleeHit(aim));
                    break;
            }           
        }
    }

    public void Heal(IPlayer aim)
    {
        if (isReadyToHit)
        {
            _timer = 0;

            switch (character.CharacterTypeByUniqueName)
            {
                case CharacterTypesByUniqueName.PriestSimpleHuman:
                    StartCoroutine(heal(aim));
                    break;
            }
        }
    }

    private IEnumerator heal(IPlayer aim)
    {
        WaitForSeconds w = new WaitForSeconds(0.05f);

        transform.LookAt(new Vector3(aim.PlayerTransform.position.x, transform.position.y, aim.PlayerTransform.position.z));
        _animator.Hit();
        myPlayer.SetBusy(true);
        yield return new WaitForSeconds(0.15f);

        GameObject g = effects.HealEffectPool.GetObject();
        aim.ReceiveHeal(myPlayer);
        myPlayer.SetBusy(false);
        g.transform.position = aim.PlayerTransform.position;
        g.SetActive(true);

        for (float i = 0; i < 0.4f; i +=0.05f)
        {
            g.transform.position = aim.PlayerTransform.position;
            yield return w;
        }

        effects.HealEffectPool.ReturnObject(g);
    }

    private IEnumerator meleeHit(IPlayer aim)
    {
        transform.LookAt(new Vector3(aim.PlayerTransform.position.x, transform.position.y, aim.PlayerTransform.position.z));
        _animator.Hit();
        myPlayer.SetBusy(true);
        yield return new WaitForSeconds(0.1f);

        GameObject g = assets.MeleeSoundsPool.GetObject();
        g.transform.position = transform.position;
        g.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        aim.ReceiveHit(myPlayer, registerKilling);

        yield return new WaitForSeconds(0.1f);
        myPlayer.SetBusy(false);

        yield return new WaitForSeconds(0.2f);
        assets.MeleeSoundsPool.ReturnObject(g);
    }

    private IEnumerator bowHit(IPlayer aim)
    {
        float maxFlyTime = 0.2f;

        transform.LookAt(new Vector3(aim.PlayerTransform.position.x, transform.position.y, aim.PlayerTransform.position.z));
        _animator.Hit();
        myPlayer.SetBusy(true);
        yield return new WaitForSeconds(0.3f);
        
        GameObject g = assets.BowSoundsPool.GetObject();
        g.transform.position = transform.position;
        g.SetActive(true);

        GameObject arrow = assets.Arrow1Pool.GetObject();
        arrow.SetActive(true);
        Vector3 myPoint = transform.position + Vector3.up * 0.8f + transform.forward * 0.3f;
        arrow.transform.position = myPoint;
        
        Vector3 upV = Vector3.up;
        switch(aim.Character.Size)
        {
            case CharacterSized.small:
                upV *= 1;
                break;

            case CharacterSized.medium:
                upV *= 2;
                break;

            case CharacterSized.big:
                upV *= 3;
                break;
        }

        Vector3 aimPoint = aim.PlayerTransform.position + upV;
        arrow.transform.LookAt(aimPoint);
        
        float distance = (aimPoint - transform.position).magnitude;
        float flyTime = distance / character.HitRadius * maxFlyTime;
        flyTime = flyTime > maxFlyTime ? maxFlyTime : flyTime;

        aimPoint = Vector3.Lerp(myPoint, aimPoint, 0.9f);
        arrow.transform.DOMove(aimPoint, flyTime).SetEase(Ease.Linear);
        myPlayer.SetBusy(false);
        yield return new WaitForSeconds(flyTime);        

        //if (character.CharacterTypeByUniqueName == CharacterTypesByUniqueName.ShooterMike) print("SHOOOOOOOOOOOTTTTTTT - " + _timer + "  -  " + _timer2);
        aim.ReceiveHit(myPlayer, registerKilling);
        assets.Arrow1Pool.ReturnObject(arrow);

        yield return new WaitForSeconds(0.2f);
        assets.BowSoundsPool.ReturnObject(g);
    }
}
