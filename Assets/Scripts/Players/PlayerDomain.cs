using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerDomain : MonoBehaviour
{
    public bool IsReadyForAction { get; private set; }
    public bool SetReadyForAction(bool isReady) => IsReadyForAction = isReady;
    public int TeamID { get; private set; } = 1;
    public float BorderRadius { get; private set; } = 4.5f;
    public PlayerTypes PlayerType { get; private set; }
    public List<CharacterManager> PlayerAims => characterAimer.Aims;
    public float CurrentSpeed { get; private set; }


    private NavMeshAgent agent;
    private List<CharacterManager> allSquad = new List<CharacterManager>();
    private CapsuleCollider _collider;
    
    private AssetManager assets;
    private CharacterAimer characterAimer;

    private float agentSpeed;
        
    public void SetData(int team, float radius, PlayerTypes _type)
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;

        if (_collider == null) _collider = GetComponent<CapsuleCollider>();

        TeamID = team;
        BorderRadius = radius;
        PlayerType = _type;

        _collider.radius = BorderRadius;
        if (TryGetComponent(out CharacterAimer c))
        {
            characterAimer = c;
            c.SetData(BorderRadius, TeamID);
        }

    }

    private void Update()
    {
        CurrentSpeed = agent.velocity.magnitude;

        if (agent.speed != agentSpeed)
        {
            agentSpeed = agent.speed;

            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].SetSpeed(agentSpeed);
            }
        }
    }


    public void AddCharacter(Character c)
    {
        //GameObject g = Instantiate(Resources.Load<GameObject>("CharacterManager"), transform);
        //CharacterManager m = g.GetComponent<CharacterManager>();
        //CharacterManager m = factory.Create();
        CharacterManager m = Instantiate(assets.CharacterManagerPool.GetObject(), transform).GetComponent<CharacterManager>();
        m.gameObject.SetActive(true);
        m.transform.parent = transform;
        m.transform.localPosition = Vector3.zero;
        m.gameObject.name = c.Name;
        m.SetCharacter(c, TeamID, agent.speed, Character.GetCharacterObject(c.CharacterTypes), this);
        allSquad.Add(m);

        List<Vector3> pos = getPositionDeltaByCount(allSquad.Count);
        for (int i = 0; i < allSquad.Count; i++)
        {
            allSquad[i].WalkToPoint(pos[i] / 1.5f + transform.position);
        }
    }

    public void WalkToPoint(Vector3 _point)
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(_point);

        if (allSquad.Count > 0)
        {
            List<Vector3> pos = getPositionDeltaByCount(allSquad.Count);
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].WalkToPoint(pos[i]/1.5f + _point);
            }
        }        
    }

    private List<Vector3> getPositionDeltaByCount(int count)
    {
        List<Vector3> result = new List<Vector3> ();
        
        switch(count)
        {
            case 0:
                result.Add(Vector3.zero);
                break;

            case 1:
                result.Add(Vector3.zero);
                break;

            case 2:
                result.Add(new Vector3(-1, 0, -1));
                result.Add(new Vector3(1, 0, 1));
                break;

            case 3:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-1, 0, -1));
                result.Add(new Vector3(1, 0, 1));
                break;
        }

        return result;
    }
        
}

public enum PlayerTypes
{
    main_player,
    player,
    npc
}
