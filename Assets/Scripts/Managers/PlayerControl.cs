using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{

    [Header("Controls")]
    private AnimationControl animationControl;


    //INPUT
    public float angleYForMobile { get; private set; }    
    public void SetVectorMovements(Vector2 vec) => vectorMovements = vec;
    public void SetRotationAngle(float ang) => angleY = ang;
    public Action JumpActivator { get; private set; }
    public Action AttackActivator { get; private set; }

    private float angleY;
    private Vector2 vectorMovements;
    private bool isForward;
    private bool isSolidPlayerBodyON;


    //SPEED
    public float PlayerMaxSpeed { get; private set; }
    public float PlayerCurrentSpeed { get; private set; }
    public float PlayerVelocity { get; private set; }
    public float PlayerNonVerticalVelocity { get; private set; }
    public float PlayerVerticalVelocity { get; private set; }

    //CONDITIONS
    public bool IsGrounded { get; private set; } = true;
    public bool IsCanJump { get; private set; }
    public bool IsCanWalk { get; private set; }
    public bool IsSpeedChanged { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsIdle { get; private set; }
    public bool IsHit { get; private set; }


    public bool IsFloating { get; private set; }
    public bool IsCanAct { get; private set; }
    public bool IsDead { get; private set; }

    private Rigidbody _rigidbody;
    private CapsuleCollider mainCollider;
    private Transform _transform;

    private float jumpCooldown;
    private float howLongNonGrounded;
    private float howLongMoving;
    private float checkGroundTimer;

    private LayerMask ignoreMask;

    private WaitForSeconds ZeroOne = new WaitForSeconds(0.1f);
    private WaitForSeconds FixedOne = new WaitForSeconds(0.02f);


    private void OnEnable()
    {
        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player" });

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = PhysicsCustomizing.GetData(PhysicObjects.Player).Mass;
        _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
        _rigidbody.angularDrag = PhysicsCustomizing.GetData(PhysicObjects.Player).AngularDrag;

        _transform = GetComponent<Transform>();
        mainCollider = GetComponent<CapsuleCollider>();
        PlayerMaxSpeed = Globals.BASE_SPEED;
        PlayerCurrentSpeed = PlayerMaxSpeed;
        IsCanAct = true;
        IsCanJump = true;
        IsCanWalk = true;

        _rigidbody.MovePosition(Vector3.zero + Vector3.up * 10);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out AnimationControl a))
            {
                animationControl = a;
                animationControl.SetPlayerControl(this);
            }
        }

        JumpActivator = makeJump;
        AttackActivator = makeHit;
    }

    

    public void StopJumpPermission(float seconds)
    {
        StartCoroutine(jumpPermission(seconds));
    }
    private IEnumerator jumpPermission(float seconds)
    {
        IsCanJump = false;

        for (float i = 0; i < seconds; i += 0.1f)
        {
            yield return ZeroOne;
            if (IsDead) break;
        }

        IsCanJump = true;
    }

    public void ChangeSpeed(float multiplier, float seconds)
    {
        StartCoroutine(changeSpeed(multiplier, seconds));
    }
    private IEnumerator changeSpeed(float multiplier, float seconds)
    {
        PlayerCurrentSpeed *= multiplier;
        IsSpeedChanged = true;

        for (float i = 0; i < seconds; i += 0.1f)
        {
            yield return ZeroOne;
            if (IsDead) break;
        }

        IsSpeedChanged = false;
        PlayerCurrentSpeed /= multiplier;
    }



    private void Update()
    {        
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;




        if (isForward && IsCanWalk)
        {
            movement(true);
        }
        else
        {
            movement(false);
        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (checkGroundTimer >= Time.fixedDeltaTime * 2)
        {
            checkGroundTimer = 0;
            IsGrounded = checkGround();
        }
        else
        {
            checkGroundTimer += Time.fixedDeltaTime;
        }

        //effectsControl.WalkSmoke(IsGrounded);

        PlayerVelocity = _rigidbody.velocity.magnitude;
        PlayerNonVerticalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;
        PlayerVerticalVelocity = new Vector3(0, _rigidbody.velocity.y, 0).magnitude;

        if (!IsGrounded)
        {
            howLongNonGrounded += Time.deltaTime;

            GravityScale(_rigidbody);
        }
        else
        {
            howLongNonGrounded = 0;

            if (_rigidbody.drag != PhysicsCustomizing.GetData(PhysicObjects.Player).Drag) _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
        }

        //if (isJump) makeJump();
    }

    private void makeHit()
    {        
        if (!IsCanAct || !IsGrounded || IsHit) return;

        playHit().Forget();
    }
    private async UniTask playHit()
    {
        IsHit = true;
        await animationControl.Hit();
        IsHit = false;
    }


    private void makeJump()
    {        
        if (!IsCanAct) return;

        if (IsGrounded && jumpCooldown <= 0 && !IsJumping && IsCanJump)
        {
            float addKoef = 1;

            _rigidbody.velocity = Vector3.zero;
            //effectsControl.MakeJumpFX();
            animationControl.JumpStart();

            _rigidbody.AddRelativeForce(Vector3.up * Globals.JUMP_POWER * addKoef, ForceMode.Impulse);
            IsJumping = true;
            jumpCooldown = 0.2f;
        }
    }

    private bool checkGround()
    {
        bool result = Physics.CheckBox(_transform.position + Vector3.down * 0f, new Vector3(0.25f, 0.05f, 0.25f), _transform.rotation, ~ignoreMask, QueryTriggerInteraction.Ignore);

        if (!IsGrounded && result)
        {
            //effectsControl.MakeLandEffect();
        }

        if (!IsGrounded && result)
        {
            IsRunning = false;
            IsIdle = true;
        }

        if (result)
        {
            if (jumpCooldown <= 0) IsJumping = false;
            IsFloating = false;
        }

        return result;
    }


    private void movement(bool forward)
    {
        if (!IsCanAct || !IsCanWalk)
        {
            return;
        }


        if (Mathf.Abs(vectorMovements.x) > 0 || Mathf.Abs(vectorMovements.y) > 0 || forward || Mathf.Abs(angleY) > 0)
        {
            float turnKoeff = PlayerCurrentSpeed * 0.03f;

            
            if (Mathf.Abs(vectorMovements.x) > 0 || Mathf.Abs(vectorMovements.y) > 0)
            {
                if (Mathf.Abs(angleY) > 0)
                {
                    angleYForMobile += angleY;
                }

                float angle = Mathf.Atan2(vectorMovements.x, vectorMovements.y) * 180 / Mathf.PI;
                
                if (Globals.IsMobile)
                {
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z), Time.deltaTime * 10);
                }
                else
                {
                    _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z)));
                }

            }
            else if (vectorMovements.x == 0 && vectorMovements.y == 0 && Mathf.Abs(angleY) > 0)
            {
                angleYForMobile += angleY;


                if (Globals.IsMobile)
                {
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), Time.deltaTime * 10);
                }
                else
                {
                    _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z)));
                }

            }


            float forAngle = angleY;
            angleY = 0;



            if (forward)
            {
                forward = false;
                //vertical = 1;
                vectorMovements = new Vector2(vectorMovements.x, 1);
            }

            float addSpeedKoeff = 1f;

            if (Globals.IsMobile)
            {
                addSpeedKoeff += Globals.SPEED_INC_IN_NONGROUND_MOBILE;
            }
            else
            {
                addSpeedKoeff += Globals.SPEED_INC_IN_NONGROUND_PC;
            }


            if ((PlayerVelocity < PlayerCurrentSpeed && IsGrounded) || (PlayerVelocity < PlayerCurrentSpeed * addSpeedKoeff && !IsGrounded))
            {
                float koeff = 0;
                float addKoeff = IsGrounded ? 1 : addSpeedKoeff;

                koeff = PlayerCurrentSpeed * addKoeff * new Vector2(vectorMovements.x, vectorMovements.y).magnitude - PlayerVelocity;

                koeff = koeff > 0 ? koeff : 0;

                _rigidbody.velocity += (_transform.forward) * koeff;

            }

            howLongMoving = 0;
            vectorMovements = Vector2.zero;
        }
        else
        {


            if (howLongMoving < 2 && IsGrounded)
            {
                howLongMoving++;
                _rigidbody.velocity = Vector3.zero;
            }

        }

        if (isSolidPlayerBodyON && _transform.position.y < 0.1f)
        {
            _rigidbody.MovePosition(new Vector3(_transform.position.x, 0.15f, _transform.position.z));
        }
    }


    private void GravityScale(Rigidbody r)
    {
        if (!r.useGravity)
        {
            r.useGravity = true;
        }

        float fallingKoeff = 1;

        if (r.velocity.y >= 0)
        {
            r.AddForce(Physics.gravity * Globals.GRAVITY_KOEFF * r.mass);
        }
        else if (r.velocity.y < 0)
        {
            if (!IsFloating)
            {
                if (r.drag != 2) r.drag = 2;
                if (fallingKoeff < 8) fallingKoeff *= 1.2f;
                r.AddForce(Physics.gravity * r.mass * Globals.GRAVITY_KOEFF * fallingKoeff);
            }
            else
            {
                //if (r.drag != 3) r.drag = 3;
                r.AddRelativeForce(Vector3.forward * 20, ForceMode.Force);
                r.AddForce(Physics.gravity * r.mass * Globals.GRAVITY_KOEFF * 0.1f);
            }
        }

    }

}

public struct PhysicsCustomizing
{
    public float Mass;
    public float Drag;
    public float AngularDrag;

    public static PhysicsCustomizing GetData(PhysicObjects _type)
    {
        PhysicsCustomizing result = new PhysicsCustomizing();
        switch (_type)
        {
            case PhysicObjects.Player:
                result.Mass = Globals.MASS;
                result.Drag = Globals.DRAG;
                result.AngularDrag = Globals.ANGULAR_DRAG;
                break;

        }

        return result;
    }




}

public enum PhysicObjects
{
    Player,
    Ragdoll
}

public enum ApplyForceType
{
    Punch_easy,
    Punch_medium,
    Punch_large
}



public enum HitType
{
    None,
    Chop,
    Mine
}
