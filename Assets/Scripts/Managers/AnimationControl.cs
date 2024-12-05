using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public AnimationStates AnimationState { get; private set; }

    private Animator _animator;
    private PlayerControl pc;
    private float speedLimit = 1f;
    private float minSpeed = 0.1f;
    private float _timer = 0.1f;
    private readonly float coolDown = 0.1f;

    public void SetPlayerControl(PlayerControl p) => pc = p;
        

    private void Start()
    {
        _animator = GetComponent<Animator>();  
        AnimationState = AnimationStates.None;
        idle();
    }

    public async UniTask Hit()
    {
        if (AnimationState == AnimationStates.Hit) return;
        AnimationState = AnimationStates.Hit;

        _animator.SetLayerWeight(1, 1);
        _animator.Play("Hit");

        int awaited = 0;
        bool isHit = false;

        print("START");

        await UniTask.Delay(100);

        while (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
        {
            awaited += 30;
            await UniTask.Delay(30);

            //print(awaited);

            if (awaited > 200 && !isHit)
            {
                isHit = true;
                
            }
        }
        print("END");

        _animator.SetLayerWeight(1, 0);
        AnimationState = AnimationStates.Idle;
        checkMovement();
    }

    public void Run()
    {
        checkMovement();
    }

    public void Idle()
    {
        checkMovement();
    }


    public void JumpStart()
    {
        _animator.Play("JumpStart");
        AnimationState = AnimationStates.Fly;
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
        if (pc.IsGrounded /*&& AnimationState != AnimationStates.Hit && AnimationState != AnimationStates.Collect*/)
        {
            float speed = pc.PlayerVelocity;
                        
            if (speed < speedLimit && speed > minSpeed)
            {
                walk();
            }
            else if (speed > speedLimit)
            {
                run();
            }
            else if (speed <= minSpeed)
            {
                idle();
            }
        }
        else if (!pc.IsGrounded)
        {            
            fly();
        }
        
    }

    private void fly()
    {
        if (AnimationState == AnimationStates.Fly) return;
            AnimationState = AnimationStates.Fly;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")
                && !_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpStart")) _animator.Play("JumpLoop");
    }

    private void idle()
    {
        if (AnimationState == AnimationStates.Idle) return;
            AnimationState = AnimationStates.Idle;
                
        
        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {            
            _animator.Play("Idle");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }
    }

    private void run()
    {
        if (AnimationState == AnimationStates.Run) return;
            AnimationState = AnimationStates.Run;
        
        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }

    }

    private void walk()
    {
        if (AnimationState == AnimationStates.Walk) return;
            AnimationState = AnimationStates.Walk;

        if (pc.IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _animator.Play("Walk");
        }
        else if (!pc.IsGrounded)
        {
            fly();
        }
    }



}

public enum AnimationStates
{
    None,
    Idle,
    Run,
    Fly,
    Walk,
    Hit,
    Collect
}
