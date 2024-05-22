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

    public void SetData(Character c, AnimationControl a)
    {
        character = c;
        _animator = a;
        _timer = attackSpeed+0.1f;

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

    public void Hit(CharacterManager aim)
    {
        if (isReadyToHit)
        {            
            transform.LookAt(new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z));

            _animator.Hit();
            _timer = 0;
        }
    }
}
