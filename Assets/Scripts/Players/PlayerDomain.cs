using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using Zenject;

public class PlayerDomain : MonoBehaviour
{
    [Inject] private DynamicUI dynamicUI;
    public bool IsReadyForAction { get; private set; }
    public bool SetReadyForAction(bool isReady)
    {
        IsReadyForAction = isReady;

        if (allSquad.Count > 0)
        {            
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].IsReadyForAction = isReady;
            }
        }

        return IsReadyForAction;
    }
        
    public int TeamID { get; private set; } = 1;
    public float BorderRadius { get; private set; } = 5f;
    public PlayerTypes PlayerType { get; private set; }
    public List<CharacterManager> PlayerAims => characterAimer.Aims;
    public float CurrentSpeed { get; private set; }
    public float MaxAgentSpeed { get; private set; }
    

    private NavMeshAgent agent;
    private List<CharacterManager> allSquad = new List<CharacterManager>();
    private CapsuleCollider _collider;
    
    private AssetManager assets;
    private CharacterAimer characterAimer;
        
    private bool isReadyForActionLastState;

    private int index = 0;
    private float _timer;


    public void SetData(int team, float radius, float speed, PlayerTypes _type)
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        MaxAgentSpeed = speed;

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


        if (!IsReadyForAction && isReadyForActionLastState)
        {
            isReadyForActionLastState = false;

            List<Vector3> pos = getPositionDeltaByCount(allSquad.Count);
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].WalkToPoint(pos[i] + transform.position);
            }

        }
        else if (IsReadyForAction && !isReadyForActionLastState)
        {
            isReadyForActionLastState = true;
        }

        if (_timer > 1)
        {
            _timer = 0;

        }
        else
        {
            _timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            for (int i = 0; i < allSquad.Count; i++)
            {
                print(i + " = " + allSquad[i].gameObject.name);
            }
        }
    }

    public void RemoveCharacter(CharacterManager character)
    {
        allSquad.Remove(character);
        character.transform.parent = transform.parent;
        StartCoroutine(playRemoveCharacter(character));
    }
    private IEnumerator playRemoveCharacter(CharacterManager character)
    {
        yield return new WaitForSeconds(Globals.PLAYER_DEATH_WAIT_ANIMATION/2f);
        assets.SetGrave(character.transform.position, character.Character.CharacterTypeByCathegory);
        yield return new WaitForSeconds(Globals.PLAYER_DEATH_WAIT_ANIMATION / 2f);
        character.gameObject.SetActive(false);
        assets.CharacterManagerPool.ReturnObject(character.gameObject);
    }

    public CharacterManager AddCharacter(Character c)
    {   
        CharacterManager m = assets.CharacterManagerPool.GetObject().GetComponent<CharacterManager>();
        m.gameObject.SetActive(true);
        m.transform.parent = transform;
        m.transform.localPosition = Vector3.zero;
        m.gameObject.name = c.Name + index;
        index++;
        m.SetCharacter(c, TeamID, MaxAgentSpeed, Character.GetCharacterObject(c.CharacterTypeByUniqueName), PlayerType, RemoveCharacter, this);
        m.PlayerAims = characterAimer.Aims;
        allSquad.Add(m);

        List<Vector3> pos = getPositionDeltaByCount(allSquad.Count);
        for (int i = 0; i < allSquad.Count; i++)
        {
            allSquad[i].WalkToPoint(pos[i] + transform.position);
        }

        dynamicUI.AddCharacter(m, true);
        return m;
    }

    public void WalkToPoint(Vector3 _point)
    {
        if (agent.isStopped) agent.isStopped = false;

        if (PlayerType == PlayerTypes.npc)
        {
            agent.SetDestination(_point);
        }
        else
        {
            agent.SetDestination(_point);

            if (allSquad.Count > 0)
            {
                List<Vector3> pos = getPositionDeltaByCount(allSquad.Count);
                for (int i = 0; i < allSquad.Count; i++)
                {
                    allSquad[i].WalkToPoint(pos[i] + _point);
                }
            }
        }
    }

    public void AddCollectableObject(CollectableObjects obj, GameObject g)
    {
        print("Added " + obj);
        g.SetActive(false);
        assets.ShowConsumeGold(g.transform.position + Vector3.up * 0.5f);
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
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                break;

            case 3:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                break;

            case 4:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                break;

            case 5:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                break;

            case 6:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                break;

            case 7:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                break;

            case 8:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                break;

            case 9:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                result.Add(new Vector3(0, 0, -1.5f));
                break;

            case 10:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                result.Add(new Vector3(0, 0, -1.5f));
                result.Add(new Vector3(1.5f, 0, -1.5f));
                break;

            case 11:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                result.Add(new Vector3(0, 0, -1.5f));
                result.Add(new Vector3(1.5f, 0, -1.5f));
                result.Add(new Vector3(-1.5f, 0, 1.5f));
                break;

            case 12:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                result.Add(new Vector3(0, 0, -1.5f));
                result.Add(new Vector3(1.5f, 0, -1.5f));
                result.Add(new Vector3(-1.5f, 0, 1.5f));
                result.Add(new Vector3(1.5f, 0, 1.5f));
                break;

            case 13:
                result.Add(Vector3.zero);
                result.Add(new Vector3(-0.75f, 0, -0.75f));
                result.Add(new Vector3(0.75f, 0, 0.75f));
                result.Add(new Vector3(-0.75f, 0, 0.75f));
                result.Add(new Vector3(0.75f, 0, -0.75f));
                result.Add(new Vector3(1.5f, 0, 0));
                result.Add(new Vector3(-1.5f, 0, 0));
                result.Add(new Vector3(0, 0, 1.5f));
                result.Add(new Vector3(0, 0, -1.5f));
                result.Add(new Vector3(1.5f, 0, -1.5f));
                result.Add(new Vector3(-1.5f, 0, 1.5f));
                result.Add(new Vector3(1.5f, 0, 1.5f));
                result.Add(new Vector3(-1.5f, 0, -1.5f));
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

public enum CollectableObjects
{
    goldCoin,
    gem
}
