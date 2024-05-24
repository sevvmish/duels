using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAttack : MonoBehaviour
{    
    private AssetManager assets;
    private Character character;
    private AnimationControl _animator;
    private CharacterManager characterManager;
    private float attackSpeed => character.AttackSpeed;
    private float currentDamage => character.CurrentDamage;

    private float _timer, _timer2;
    private bool isReadyToHit => _timer >= attackSpeed;

    private Action<Character> registerKilling;

    private void Start()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
    }

    public void SetData(CharacterManager characterM, Action<Character> k)
    {
        characterManager = characterM;
        character = characterM.Character;
        _animator = characterM.CharacterAnimator;
        _timer = attackSpeed + 0.1f;
        registerKilling = k;
    }

    private void Update()
    {
        _timer2 += Time.deltaTime;

        if (_timer >= attackSpeed)
        {            
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void Hit(CharacterManager aim)
    {
        //if (character.CharacterTypeByUniqueName == CharacterTypesByUniqueName.ShooterMike) print(isReadyToHit + " !!!! timer: " + _timer + " =  att speed: " + attackSpeed);

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
            }

            

        }
    }

    private IEnumerator meleeHit(CharacterManager aim)
    {
        transform.LookAt(new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z));
        _animator.Hit();
        characterManager.SetBusy(true);
        yield return new WaitForSeconds(0.1f);

        GameObject g = assets.MeleeSoundsPool.GetObject();
        g.transform.position = transform.position;
        g.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        aim.ReceiveHit(characterManager, registerKilling);

        yield return new WaitForSeconds(0.1f);
        characterManager.SetBusy(false);

        yield return new WaitForSeconds(0.2f);
        assets.MeleeSoundsPool.ReturnObject(g);
    }

    private IEnumerator bowHit(CharacterManager aim)
    {
        float maxFlyTime = 0.2f;

        transform.LookAt(new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z));
        _animator.Hit();
        characterManager.SetBusy(true);
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

        Vector3 aimPoint = aim.transform.position + upV;
        arrow.transform.LookAt(aimPoint);
        
        float distance = (aimPoint - transform.position).magnitude;
        float flyTime = distance / character.HitRadius * maxFlyTime;
        flyTime = flyTime > maxFlyTime ? maxFlyTime : flyTime;

        aimPoint = Vector3.Lerp(myPoint, aimPoint, 0.9f);
        arrow.transform.DOMove(aimPoint, flyTime).SetEase(Ease.Linear);
        characterManager.SetBusy(false);
        yield return new WaitForSeconds(flyTime);        

        //if (character.CharacterTypeByUniqueName == CharacterTypesByUniqueName.ShooterMike) print("SHOOOOOOOOOOOTTTTTTT - " + _timer + "  -  " + _timer2);
        aim.ReceiveHit(characterManager, registerKilling);
        assets.Arrow1Pool.ReturnObject(arrow);

        yield return new WaitForSeconds(0.2f);
        assets.BowSoundsPool.ReturnObject(g);
    }
}
