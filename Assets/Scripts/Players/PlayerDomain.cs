using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerDomain : MonoBehaviour
{
    [Inject] private DynamicUI dynamicUI;
    [Inject] private Sounds sounds;
    [Inject] private GameManager gm;

    public int GoldCoins {  get; private set; }
    public int Gems { get; private set; }
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

    public bool IsCanMove { get; private set; } = true;
        
    public int TeamID { get; private set; } = 1;
    public float BorderRadius { get; private set; } = 5f;
    public PlayerTypes PlayerType { get; private set; }    
    public float CurrentSpeed { get; private set; }
    public float MaxAgentSpeed { get; private set; }
    public List<CharacterManager> PlayerSquad { get => allSquad; }
    public int PlayerSquadAmount { get => allSquad.Count; }
    public bool IsItMainPlayer;


    private NavMeshAgent agent;
    private List<CharacterManager> allSquad = new List<CharacterManager>();
    private CapsuleCollider _collider;
    
    private AssetManager assets;
    private EffectsManager effects;
    public CharacterAimer CharacterAimer { get; private set; }

    private bool isReadyForActionLastState;

    private int index = 0;
    private float _timer;


    public void SetData(int team, float radius, float speed, PlayerTypes _type)
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        effects = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();

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
            CharacterAimer = c;
            c.SetData(BorderRadius, TeamID);
        }
    }

    private void Update()
    {
        CurrentSpeed = agent.velocity.magnitude;


        if (!IsReadyForAction && isReadyForActionLastState)
        {
            isReadyForActionLastState = false;

            List<Vector3> pos = GetPositionDeltaByCount(allSquad.Count);
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].WalkToPoint(pos[i] + transform.position);
            }

        }
        else if (IsReadyForAction && !isReadyForActionLastState)
        {
            isReadyForActionLastState = true;
        }

        if (_timer > Globals.COOLDOWN_CHECK_DEAD_PLAYERS)
        {
            _timer = 0;

            for (int i = 0; i < allSquad.Count; i++)
            {
                if (!allSquad[i].Character.IsAlive)
                {
                    RemoveCharacter(allSquad[i]);
                }
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }

        
    }

    public void SetMoveAbility(bool isCanMove) => IsCanMove = isCanMove;

    public void RemoveCharacter(CharacterManager character)
    {
        allSquad.Remove(character);
        character.transform.parent = transform.parent;
        StartCoroutine(playRemoveCharacter(character));
    }
    private IEnumerator playRemoveCharacter(CharacterManager character)
    {
        yield return new WaitForSeconds(Globals.PLAYER_DEATH_WAIT_ANIMATION/2f);
        effects.SetGrave(character.transform.position, character.Character.CharacterTypeByCathegory);
        yield return new WaitForSeconds(Globals.PLAYER_DEATH_WAIT_ANIMATION / 2f);
        
        GameObject g = character.GetCharacterGameobject;
        if (g.TryGetComponent(out CharacterAimer a))
        {
            Destroy(a);
        }
        if (g.TryGetComponent(out CapsuleCollider c))
        {
            Destroy(c);
        }        

        assets.ReturnCharacterObject(character.GetCharacterGameobject, character.Character.CharacterTypeByUniqueName);
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
        m.SetCharacter(c, TeamID, MaxAgentSpeed, assets.GetCharacterObject(c.CharacterTypeByUniqueName), RemoveCharacter, this);
        m.SetAimer(CharacterAimer);
        allSquad.Add(m);

        List<Vector3> pos = GetPositionDeltaByCount(allSquad.Count);
        for (int i = 0; i < allSquad.Count; i++)
        {
            allSquad[i].WalkToPoint(pos[i] + transform.position);
        }

        dynamicUI.AddCharacter(m, true);
        effects.SetNewPlayerSpawnEffect(m.transform);
        return m;
    }

    public void WalkToPoint(Vector3 _point)
    {
        if (!IsCanMove) return;

        if (agent.isStopped) agent.isStopped = false;

        agent.SetDestination(_point);

        if (allSquad.Count > 0)
        {
            List<Vector3> pos = GetPositionDeltaByCount(allSquad.Count);
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].WalkToPoint(pos[i] + _point);
            }
        }
    }

    public void AddCollectableObject(InteractableObjects obj, GameObject g)
    {
        print("Added " + obj);

        switch(obj)
        {
            case InteractableObjects.goldCoin:
                GoldCoins++;
                effects.ConsumeGold(g);
                break;

            case InteractableObjects.portal:
                SpawnPortal portal = g.GetComponent<SpawnPortal>();
                int cost = portal.GetCost(PlayerSquadAmount);

                if (cost <= GoldCoins)
                {
                    GoldCoins -= cost;
                    gm.TurnOnChoseCharacterProcess(IsItMainPlayer, portal);
                }
                
                break;
        }
        
        
    }





    public static List<Vector3> GetPositionDeltaByCount(int count)
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

public enum InteractableObjects
{
    goldCoin,
    gem,
    portal
}
