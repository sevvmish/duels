using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CharacterManager : MonoBehaviour
{
    public bool IsReadyForAction;
    private float MaxAgentSpeed = 5;
    public List<CharacterManager> PlayerAims;

    public Character Character { get; private set; }
    public float CurrentSpeed { get; private set; }
    public void SetSpeed(float speed) => agent.speed = speed;
    public int TeamID { get; private set; }
    public AnimationControl CharacterAnimator { get; private set; }
    public PlayerTypes PlayerType { get; private set; }

    private NavMeshAgent agent;    
    private CapsuleCollider _collider;
    private bool isAttacking;
    private PerformAttack performAttack;
    PlayerDomain d;


    public void SetCharacter(Character c, int team, float speed, GameObject characterObject, List<CharacterManager> aims, PlayerTypes p, PlayerDomain d)
    {
        PlayerType = p;
        this.d = d;

        PlayerAims = aims;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        MaxAgentSpeed = speed;
        _collider = GetComponent<CapsuleCollider>();

        Character = c;
        GameObject g = Instantiate(characterObject, transform);        
        CharacterAnimator = g.GetComponent<AnimationControl>();
        CharacterAnimator.SetData(this);
        TeamID = team;
        _collider.radius = c.DamageRadius / 2f;
        g.SetActive(true);

        gameObject.AddComponent<PerformAttack>();
        performAttack = GetComponent<PerformAttack>();
        performAttack.SetData(Character, CharacterAnimator);
    }


    public void WalkToPoint(Vector3 _point)
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(_point);
    }
        
       

    private void Update()
    {       
        CurrentSpeed = agent.velocity.magnitude;

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

    private void sendToAttack()
    {
        isAttacking = true;
        
        float minDist = float.MaxValue;
        CharacterManager aim = null;

        for (int i = 0; i < PlayerAims.Count; i++)
        {
            if (!PlayerAims[i].Character.IsAlive) continue;

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
                if (PlayerType == PlayerTypes.npc)
                {
                    d.WalkToPoint(aim.transform.position);
                }
                else
                {
                    WalkToPoint(aim.transform.position);
                }
                    
            }
        }
    }

}
