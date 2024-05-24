using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public AnimationStates AnimationState { get; private set; }

    private Animator _animator;
    private float speedLimitWalkRun = 2f;
    private float minSpeed = 0.1f;
    private float _timer;
    private readonly float coolDown = 0.1f;

    private CharacterManager cm;    

    private void OnEnable()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    public void SetData(CharacterManager c)
    {
        cm = c;
    }

    public void Run()
    {
        checkMovement();
    }

    
    public void Idle()
    {
        checkMovement();
    }

    public void Hit()
    {
        if (!cm.Character.IsAlive) die();
        hit();
    }


    private void Update()
    {
        if (cm == null) return;

        if (!cm.Character.IsAlive) die();
        
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        _timer = coolDown;
        
        checkMovement();
    }

    private void checkMovement()
    {
        float speed = cm.CurrentSpeed;

        /*
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && (int)AnimationState > 3)
        {
            AnimationState = AnimationStates.Idle;
        }
        */
        
        if (speed < speedLimitWalkRun && speed > minSpeed)
        {
            walk();
        }
        else if (speed > speedLimitWalkRun)
        {
            run();
        }
        else if (speed <= minSpeed)
        {
            idle();
        }
        
        

    }


    private void idle()
    {
        if (AnimationState == AnimationStates.Idle || (int)AnimationState > 3) return;
        AnimationState = AnimationStates.Idle;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.Play("Idle");
        }
    }

    private void die()
    {
        if (AnimationState == AnimationStates.Dead) return;
        AnimationState = AnimationStates.Dead;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            _animator.Play("Die");
        }
    }

    private void run()
    {
        if (AnimationState == AnimationStates.Run/* || (int)AnimationState > 3*/) return;
        AnimationState = AnimationStates.Run;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
        }
    }

    private void hit()
    {
        AnimationState = AnimationStates.Hit;
        _timer = 0.2f;
        _animator.Play("BaseHit", 0, 0);

        /*
        if (AnimationState == AnimationStates.Hit) return;
        AnimationState = AnimationStates.Hit;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("BaseHit"))
        {
            _animator.Play("BaseHit");
        }*/
    }

    private void walk()
    {
        if (AnimationState == AnimationStates.Walk/* || (int)AnimationState > 3*/) return;
        AnimationState = AnimationStates.Walk;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _animator.Play("Walk");
        }
    }
}

public enum AnimationStates
{
    None,
    Idle,
    Run,
    Walk,
    Hit,
    Dead
}
