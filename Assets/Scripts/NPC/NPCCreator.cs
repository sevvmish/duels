using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCreator : MonoBehaviour
{
    //[Inject] CharacterManager.Factory factory;
    //[Inject] private GameManager gm;

    [SerializeField] private CharacterTypesByUniqueName[] currentNPCs;
    [SerializeField] private int team = -1;
    [SerializeField] private float respawnTime = 10;

    [Header("Patrolling data")]
    [SerializeField] private PatrolingTypes patrolType = PatrolingTypes.OnPlace;
    [SerializeField] private Transform fromPoint;
    [SerializeField] private Transform toPoint;
    [SerializeField] private float radius = 10;
    [SerializeField] private float refreshPatrolRate = 10;
    private float _refreshTimer;

    private AssetManager assets;
    private DynamicUI dynamicUI;
    private List<NPCCharacter> allSquad = new List<NPCCharacter>();
    private Vector3 squadPosition;

    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        dynamicUI = GameObject.Find("DynamicCanvas").GetComponent<DynamicUI>();

        squadPosition = transform.position;

        if (currentNPCs.Length > 0)
        {
            List<Vector3> pos = PlayerDomain.GetPositionDeltaByCount(currentNPCs.Length);
            for (int i = 0; i < currentNPCs.Length; i++)
            {
                NPCCharacter c = AddCharacter(currentNPCs[i]);
                allSquad.Add(c);
                c.transform.position = squadPosition + pos[i];
            }
        }        
    }

    private void Update()
    {
        if (_refreshTimer > refreshPatrolRate)
        {
            _refreshTimer = 0;
            walkPatrolToPoint(getNextPointForPatrol(patrolType));
        }
        else
        {
            _refreshTimer += Time.deltaTime;
        }


        if (_timer > Globals.COOLDOWN_CHECK_DEAD_NPC)
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

    private void walkPatrolToPoint(Vector3 _point)
    {
        squadPosition = _point;

        if (allSquad.Count > 0)
        {
            List<Vector3> pos = PlayerDomain.GetPositionDeltaByCount(allSquad.Count);
            for (int i = 0; i < allSquad.Count; i++)
            {
                allSquad[i].WalkToPoint(pos[i] + _point);
            }
        }
    }

    private Vector3 getNextPointForPatrol(PatrolingTypes _type)
    {
        switch(_type)
        {
            case PatrolingTypes.OnPlace: return transform.position;
            case PatrolingTypes.BetweenPoints:
                float d1 = (squadPosition - fromPoint.position).magnitude;
                float d2 = (squadPosition - toPoint.position).magnitude;

                if (d1 < d2)
                {
                    return new Vector3(toPoint.position.x, transform.position.y, toPoint.position.z);
                }
                else
                {
                    return new Vector3(fromPoint.position.x, transform.position.y, fromPoint.position.z);
                }

            case PatrolingTypes.InRadius:
                Vector2 vec = new Vector2(UnityEngine.Random.Range(transform.position.x-radius, transform.position.x + radius),
                    UnityEngine.Random.Range(transform.position.z - radius, transform.position.z + radius));

                return new Vector3(vec.x, transform.position.y, vec.y);
        }

        return transform.position;
    }

    private void arrangePositions()
    {
        if (currentNPCs.Length > 0)
        {
            List<Vector3> pos = PlayerDomain.GetPositionDeltaByCount(currentNPCs.Length);
            for (int i = 0; i < allSquad.Count; i++)
            {                
                allSquad[i].WalkToPoint(squadPosition + pos[i]);
            }
        }
    }

    public NPCCharacter AddCharacter(CharacterTypesByUniqueName _type)
    {
        Character c = new Character(_type, 1);
        NPCCharacter m = assets.NpcCharacterPool.GetObject().GetComponent<NPCCharacter>();
        m.gameObject.SetActive(true);
        m.transform.parent = transform;
        m.transform.localPosition = Vector3.zero;
        m.gameObject.name = c.Name;
        GameObject g = m.SetCharacter(c, team, c.CurrentSpeed, assets.GetCharacterObject(c.CharacterTypeByUniqueName), RemoveCharacter, null);
        m.IsReadyForAction = true;

        g.AddComponent<CharacterAimer>();
        CharacterAimer aimer = g.GetComponent<CharacterAimer>();
        aimer.SetData(c.AggroRadius, team);
        m.SetAimer(aimer);

        dynamicUI.AddCharacter(m, false);
        return m;
    }

    public void RemoveCharacter(NPCCharacter character)
    {
        allSquad.Remove(character);
        StartCoroutine(playRespawnAfterSec(character.Character, respawnTime));
        StartCoroutine(playRemoveCharacter(character));
    }
    private IEnumerator playRemoveCharacter(NPCCharacter character)
    {
        yield return new WaitForSeconds(Globals.PLAYER_DEATH_WAIT_ANIMATION);
        assets.ReturnCharacterObject(character.GetCharacterGameobject, character.Character.CharacterTypeByUniqueName);
        assets.NpcCharacterPool.ReturnObject(character.gameObject);
    }

    private IEnumerator playRespawnAfterSec(Character c, float _timer)
    {
        yield return new WaitForSeconds(_timer);

        NPCCharacter npc = AddCharacter(c.CharacterTypeByUniqueName);
        allSquad.Add(npc);
        arrangePositions();
    }

}

public enum PatrolingTypes
{
    OnPlace,
    InRadius,
    BetweenPoints
}