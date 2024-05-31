using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CharacterManager : MonoBehaviour, IPlayer
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
    private Coroutine waitForWalk;
    private WaitForSeconds zeroOne = new WaitForSeconds(0.1f);
    private Vector3 newPointToWalk;

    private PerformAttack performAttack;
    private float MaxAgentSpeed = 5;
    private Action<CharacterManager> onDeath;

    private WaitForSeconds zeroTwoShort = new WaitForSeconds(0.02f);

    private PlayerDomain mainDomain;
    private AnimationControl characterAnimator;
    private GameObject characterGameobject;

    private CharacterAimer aimer;
    //private List<IPlayer> playerAims;
    private float _updateSendToAttack = 0;

    public void SetAimer(CharacterAimer aimer)
    {
        this.aimer = aimer;
        aimer.Aims.Clear();
        //playerAims = aimer.Aims;
    }

    public Transform PlayerTransform { get => transform; }
    public GameObject GetCharacterGameobject => characterGameobject;

    public GameObject SetCharacter(Character c, int team, float speed, GameObject characterObject, Action<CharacterManager> onDeath, PlayerDomain domain)
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

        gameObject.AddComponent<PerformAttack>();
        performAttack = GetComponent<PerformAttack>();
        performAttack.SetData(this, characterAnimator, RegisterEnemyKilled);

        return g;
    }

    public bool SetBusy(bool isBusy) => this.isBusy = isBusy;


    public void WalkToPoint(Vector3 _point)
    {
        if (!Character.IsAlive) return;

        if (isBusy)
        {
            /*
            if (waitForWalk == null)
            {
                waitForWalk = StartCoroutine(playBusyWaitForWalk());
            }*/

            newPointToWalk = _point;
            return;
        }

        if (agent != null && agent.isOnNavMesh && agent.isStopped) agent.isStopped = false;
        if (agent != null && agent.isOnNavMesh) agent.SetDestination(_point);
    }
    private IEnumerator playBusyWaitForWalk()
    {
        for (float i = 0; i < 1; i+=0.1f)
        {
            if (!isBusy) break;
            yield return zeroOne;
        }

        WalkToPoint(newPointToWalk);
    }
          
    
    private void Update()
    {
        if (_updateSendToAttack > 0)
        {
            _updateSendToAttack -= Time.deltaTime;
        }

        if (!Character.IsAlive || isBusy) return;

        if (IsReadyForAction)
        {            
            if (isAttacking)
            {
                agent.speed = MaxAgentSpeed * 1.5f;
            }
                

            if (Character.AttackType != AttackTypes.heal && aimer.Aims.Count > 0)
            {
                sendToAttack();
            }
            else if (Character.AttackType == AttackTypes.heal)
            {
                sendToAttack();
            }
            else
            {
                isAttacking = false;
                _updateSendToAttack = 0;
            }
            
        }
        else
        {
            isAttacking = false;            
            if (agent.speed > MaxAgentSpeed)
            {
                agent.speed -= Time.deltaTime*3;
            }
            else
            {
                agent.speed = MaxAgentSpeed;
            }                
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

        _updateSendToAttack = Globals.COOLDOWN_UPDATE_ATTACK_PLAYER;

        if (Character.AttackType != AttackTypes.heal)
        {
            dealDamage();
        }
        else
        {
            heal();
        }
    }

    private void heal()
    {        
        List<CharacterManager> chars = mainDomain.PlayerSquad;

        CharacterManager wounded = default;
        float leftHP = float.MaxValue;

        for (int i = 0; i < chars.Count; i++)
        {
            if (chars[i].Character.CurrentHP < chars[i].Character.MaxHP && (chars[i].Character.MaxHP - chars[i].Character.CurrentHP) < leftHP)
            {                
                leftHP = chars[i].Character.MaxHP - chars[i].Character.CurrentHP;
                wounded = chars[i];
            }
        }

        

        if (wounded != null)
        {            
            performAttack.Heal(wounded);
        }
    }

    private void dealDamage()
    {
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

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == 6)
        {            
            mainDomain.AddCollectableObject(InteractableObjects.goldCoin, other.gameObject);
        }
        else if (other.gameObject.layer == 7)
        {
            mainDomain.AddCollectableObject(InteractableObjects.portal, other.gameObject);
        }
    }
    

}
