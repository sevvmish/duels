using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public AnimationStates AnimationState { get; private set; }

    private Animator _animator;
    private float speedLimitWalkRun = 1f;
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


    private void Update()
    {
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
        if (AnimationState == AnimationStates.Idle) return;
        AnimationState = AnimationStates.Idle;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.Play("Idle");
        }
    }

    private void run()
    {
        if (AnimationState == AnimationStates.Run) return;
        AnimationState = AnimationStates.Run;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
        }
    }

    private void walk()
    {
        if (AnimationState == AnimationStates.Walk) return;
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
    Fly,
    Walk
}
