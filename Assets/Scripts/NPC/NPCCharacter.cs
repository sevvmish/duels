using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCCharacter : MonoBehaviour, IPlayer
{
    public bool IsReadyForAction;

    public Character Character { get; private set; }
    public float CurrentSpeed { get => agent.velocity.magnitude; }
    public void SetSpeed(float speed) => agent.speed = speed;
    public int TeamID { get; private set; }
        
    private NavMeshAgent agent;
    private CapsuleCollider _collider;

    private bool isAttacking;

    private bool isBusy;
    private WaitForSeconds zeroOne = new WaitForSeconds(0.1f);
    private Vector3 newPointToWalk;

    private PerformAttack performAttack;
    private float MaxAgentSpeed = 5;
    private Action<NPCCharacter> onDeath;

    private WaitForSeconds zeroTwoShort = new WaitForSeconds(0.02f);

    private PlayerDomain mainDomain;
    private AnimationControl characterAnimator;
    private GameObject characterGameobject;

    private CharacterAimer aimer;
    //private List<IPlayer> playerAims;

    private float _updateSendToAttack = 0;


    //TODEL
    public int enemies => aimer.Aims.Count;

    public void SetAimer(CharacterAimer aimer)
    {
        this.aimer = aimer;
        aimer.Aims.Clear();
        //playerAims = aimer.Aims;
        //playerAims.Clear();

    }

    public Transform PlayerTransform { get => transform; }
    public GameObject GetCharacterGameobject => characterGameobject;

    public GameObject SetCharacter(Character c, int team, float speed, GameObject characterObject, Action<NPCCharacter> onDeath, PlayerDomain domain)
    {
        _collider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        isAttacking = false;
        isBusy = false;
        agent.enabled = true;
        _collider.enabled = true;

        mainDomain = domain;
        this.onDeath = onDeath;
        c.OnCharacterDead = registerMyDeath;
        

        agent.speed = speed;
        MaxAgentSpeed = speed;
        

        Character = c;
        agent.radius = c.DamageRadius;
        GameObject g = Instantiate(characterObject, transform);
        characterGameobject = g;
        characterAnimator = g.GetComponent<AnimationControl>();
        characterAnimator.SetData(this);
        TeamID = team;
        _collider.radius = c.DamageRadius;
        g.transform.localPosition = Vector3.zero;
        g.transform.localEulerAngles = Vector3.zero;
        g.SetActive(true);

        //gameObject.AddComponent<PerformAttack>();
        performAttack = GetComponent<PerformAttack>();
        performAttack.SetData(this, characterAnimator, RegisterEnemyKilled);

        return g;
    }

    public bool SetBusy(bool isBust) => this.isBusy = isBust;


    public void WalkToPoint(Vector3 _point)
    {
        if (!Character.IsAlive) return;

        if (isBusy)
        {            
            newPointToWalk = _point;
            return;
        }

        if (agent != null && agent.isOnNavMesh && agent.isStopped) agent.isStopped = false;
        if (agent != null && agent.isOnNavMesh) agent.SetDestination(_point);
    }
    

    private void Update()
    {
        if (_updateSendToAttack > 0)
        {
            _updateSendToAttack -= Time.deltaTime;
        }

        if (!Character.IsAlive || isBusy) return;

        if (aimer.Aims.Count > 0 && !isAttacking)
        {
            _updateSendToAttack = 0;
            newPointToWalk = transform.position;
            isAttacking = true;
            //agent.speed = MaxAgentSpeed * 1.5f;
            sendToAttack();
        }
        else if (aimer.Aims.Count > 0 && isAttacking)
        {
            sendToAttack();
        }
        else if (aimer.Aims.Count == 0 && isAttacking)
        {
            isAttacking = false;            
        }
        else if (aimer.Aims.Count == 0 && !isAttacking && agent.speed > MaxAgentSpeed)
        {
            //agent.speed -= Time.deltaTime;
        }
    }

    private void registerMyDeath()
    {
        if (agent.enabled) agent.enabled = false;
        if (_collider.enabled) _collider.enabled = false;
        StopAllCoroutines();
        agent.speed = 0;
        IsReadyForAction = false;
        onDeath.Invoke(this);
    }


    private void RegisterEnemyKilled(Character victim)
    {
        print(victim.Name + " is killed by " + gameObject.name);
        _updateSendToAttack = 0;
    }

    public void ReceiveHit(IPlayer damager, Action<Character> killInfo)
    {
        if (!Character.IsAlive) return;

        Character.ReceiveDamage(damager.Character.CurrentDamage);

        if (!Character.IsAlive)
        {
            killInfo.Invoke(Character);
        }
        else
        {
            aimer.Aims.Add(damager);
        }
    }

    public void ReceiveHeal(IPlayer damager)
    {
        if (!Character.IsAlive) return;

        Character.ReceiveHeal(damager.Character.CurrentDamage);
    }


    private void sendToAttack()
    {
        isAttacking = true;

        if (_updateSendToAttack > 0) return;

        _updateSendToAttack = Globals.COOLDOWN_UPDATE_ATTACK_NPC;

        float minDist = float.MaxValue;
        IPlayer aim = null;

        for (int i = 0; i < aimer.Aims.Count; i++)
        {
            if (!aimer.Aims[i].Character.IsAlive)
            {
                aimer.Aims.Remove(aimer.Aims[i]);
                continue;
            }

            float distance = (aimer.Aims[i].PlayerTransform.position - transform.position).magnitude;
            if (distance < minDist)
            {
                minDist = distance;
                aim = aimer.Aims[i];
            }
        }

        if (aim != null)
        {
            float minusDist = Character.HitRadius + aim.Character.DamageRadius;

            if (minDist <= minusDist)
            {
                if (agent.hasPath && !agent.isStopped) agent.isStopped = true;
                performAttack.Hit(aim);
            }
            else
            {
                WalkToPoint(aim.PlayerTransform.position);
            }
        }
    }
}
