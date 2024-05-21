using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDomain : MonoBehaviour
{
    public int TeamID { get; private set; } = 1;
    public float BorderRadius { get; private set; } = 9;

    private NavMeshAgent agent;
    private List<CharacterManager> charsSquad = new List<CharacterManager>();
    private CapsuleCollider _collider;
    private PlayerBorderVisual border;

    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
        _collider.radius = BorderRadius / 2f;


        addCharacter(new Character(CharacterTypesByUniqueName.WarriorSam, 1));        
        addCharacter(new Character(CharacterTypesByUniqueName.ShooterMike, 1));

        if (TryGetComponent(out CharacterAimer c))
        {
            c.SetData(charsSquad, BorderRadius/2);
        }
    }

    public void AddBorderVisual()
    {
        if (border != null) return;
        border = Instantiate(Resources.Load<GameObject>("BorderVisual"), transform).GetComponent<PlayerBorderVisual>();
        border.SetSize(BorderRadius);
    }

    private void addCharacter(Character c)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("CharacterManager"), transform);
        CharacterManager m = g.GetComponent<CharacterManager>();
        m.SetCharacter(c, TeamID);
        charsSquad.Add(m);
    }

    public void WalkToPoint(Vector3 _point)
    {
        agent.SetDestination(_point);

        if (charsSquad.Count > 0)
        {
            List<Vector3> pos = getPositionDeltaByCount(charsSquad.Count);
            for (int i = 0; i < charsSquad.Count; i++)
            {
                charsSquad[i].WalkToPoint(pos[i]/1.5f + _point);
            }
        }
        
    }

    private List<Vector3> getPositionDeltaByCount(int count)
    {
        List<Vector3> result = new List<Vector3> ();
        
        switch(count)
        {
            case 0 - 1:
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
