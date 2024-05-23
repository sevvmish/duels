using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAttack : MonoBehaviour
{
    private Character character;
    private AnimationControl _animator;
    private float attackSpeed => character.AttackSpeed;
    private float currentDamage => character.CurrentDamage;

    private float _timer;
    private bool isReadyToHit => _timer > attackSpeed;

    private Action<Character> registerKilling;

    public void SetData(Character c, AnimationControl a, Action<Character> k)
    {
        character = c;
        _animator = a;
        _timer = attackSpeed+0.1f;
        registerKilling = k;
    }

    private void Update()
    {
        if (_timer > attackSpeed)
        {            
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void Hit(CharacterManager damager, CharacterManager aim)
    {
        if (isReadyToHit)
        {            
            transform.LookAt(new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z));

            _animator.Hit();

            aim.ReceiveHit(damager, registerKilling);
            _timer = 0;
        }
    }
}
