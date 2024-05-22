using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CharacterManager : MonoBehaviour
{    
    public Character Character { get; private set; }
    public float CurrentSpeed { get; private set; }
    public void SetSpeed(float speed) => agent.speed = speed;
    public int TeamID { get; private set; }
    
    private NavMeshAgent agent;
    private AnimationControl _animator;
    private CapsuleCollider _collider;
    private PlayerDomain domain;
        

    public void SetCharacter(Character c, int team, float speed, GameObject characterObject, PlayerDomain domain)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        _collider = GetComponent<CapsuleCollider>();
        this.domain = domain;

        Character = c;
        GameObject g = Instantiate(characterObject, transform);        
        _animator = g.GetComponent<AnimationControl>();
        _animator.SetData(this);
        TeamID = team;
        _collider.radius = c.DamageRadius / 2f;
        g.SetActive(true);
    }


    public void WalkToPoint(Vector3 _point)
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(_point);
    }
        
       

    private void Update()
    {
        CurrentSpeed = agent.velocity.magnitude;

        if (domain.IsReadyForAction && domain.PlayerAims.Count > 0)
        {
            sendToAttack();
        }
    }

    private void sendToAttack()
    {

        
        float minDist = float.MaxValue;
        CharacterManager characterManager = null;

        for (int i = 0; i < domain.PlayerAims.Count; i++)
        {
            float distance = (domain.PlayerAims[i].transform.position - transform.position).magnitude;
            if (distance < minDist)
            {
                minDist = distance;
                characterManager = domain.PlayerAims[i];
            }
        }

        if (characterManager != null)
        {
            float minusDist = Character.HitRadius + characterManager.Character.DamageRadius;
            
            if (minDist <= minusDist)
            {
                //WalkToPoint(characterManager.transform.position);
                if (agent.hasPath && !agent.isStopped) agent.isStopped = true;
            }
            else
            {
                //Vector3 dist = (characterManager.transform.position - transform.position).normalized;
                WalkToPoint(characterManager.transform.position);
            }
            

            
        }
    }



    /*
    private GameManager gm;

    public class Factory : PlaceholderFactory<CharacterManager> { }

    [Inject]
    public void Construct(GameManager gm)
    {
        this.gm = gm;
    }
    */
}
