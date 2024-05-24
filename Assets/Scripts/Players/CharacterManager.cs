using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CharacterManager : MonoBehaviour
{
    public bool IsReadyForAction;    
    public List<CharacterManager> PlayerAims;
    public Character Character { get; private set; }
    public float CurrentSpeed { get => agent.velocity.magnitude; }
    public void SetSpeed(float speed) => agent.speed = speed;
    public int TeamID { get; private set; }
    public AnimationControl CharacterAnimator { get; private set; }
    public PlayerTypes PlayerType { get; private set; }

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


    public GameObject SetCharacter(Character c, int team, float speed, GameObject characterObject, PlayerTypes p, Action<CharacterManager> onDeath, PlayerDomain domain)
    {
        mainDomain = domain;
        this.onDeath = onDeath;
        c.OnCharacterDead = RegisterMyDeath;
        PlayerType = p;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        MaxAgentSpeed = speed;
        _collider = GetComponent<CapsuleCollider>();

        Character = c;
        agent.radius = c.DamageRadius;
        GameObject g = Instantiate(characterObject, transform);        
        CharacterAnimator = g.GetComponent<AnimationControl>();
        CharacterAnimator.SetData(this);
        TeamID = team;
        _collider.radius = c.DamageRadius;
        g.SetActive(true);

        gameObject.AddComponent<PerformAttack>();
        performAttack = GetComponent<PerformAttack>();
        performAttack.SetData(this, RegisterEnemyDeath);

        return g;
    }

    public bool SetBusy(bool isBust) => this.isBusy = isBust;


    public void WalkToPoint(Vector3 _point)
    {
        if (!Character.IsAlive) return;

        if (isBusy)
        {
            if (waitForWalk == null)
            {
                waitForWalk = StartCoroutine(playBusyWaitForWalk());
            }

            newPointToWalk = _point;
            return;
        }

        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(_point);
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
        if (!Character.IsAlive || isBusy) return;

        if (IsReadyForAction)
        {            
            if (isAttacking)
            {
                agent.speed = MaxAgentSpeed * 1.5f;
            }
                

            if (PlayerAims.Count > 0)
            {
                sendToAttack();
            }
            else
            {
                isAttacking = false;
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

    public void RegisterMyDeath()
    {        
        if (agent.enabled) agent.enabled = false;
        if (_collider.enabled) _collider.enabled = false;
        StopAllCoroutines();
        agent.speed = 0;
        IsReadyForAction = false;
        onDeath.Invoke(this);
    }


    private void RegisterEnemyDeath(Character victim)
    {
        print(victim.Name + " is killed by " + gameObject.name);
    }

    public void ReceiveHit(CharacterManager damager, Action<Character> killInfo)
    {
        if (!Character.IsAlive) return;

        Character.ReceiveDamage(damager.Character.CurrentDamage);

        if (!Character.IsAlive)
        {
            killInfo.Invoke(Character);
        }
        else
        {
            if (PlayerType == PlayerTypes.npc) PlayerAims.Add(damager);
        }
    }


    private void sendToAttack()
    {
        isAttacking = true;
        
        float minDist = float.MaxValue;
        CharacterManager aim = null;

        for (int i = 0; i < PlayerAims.Count; i++)
        {
            if (!PlayerAims[i].Character.IsAlive)
            {
                PlayerAims.Remove(PlayerAims[i]);
                continue;
            }

            float distance = (PlayerAims[i].transform.position - transform.position).magnitude;
            if (distance < minDist)
            {
                minDist = distance;
                aim = PlayerAims[i];
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
                WalkToPoint(aim.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerType == PlayerTypes.npc) return;

        if (other.gameObject.layer == 6)
        {
            other.GetComponent<BoxCollider>().enabled = false;            
            mainDomain.AddCollectableObject(CollectableObjects.goldCoin, other.gameObject);
        }
    }
    

}
