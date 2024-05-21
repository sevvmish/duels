using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDomain : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<CharacterManager> charsSquad = new List<CharacterManager>();

    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        Character c = new Character(CharacterTypesByUniqueName.WarriorSam, 1);
        addCharacter(c);
        Character b = new Character(CharacterTypesByUniqueName.ShooterMike, 1);
        addCharacter(b);
    }

    private void addCharacter(Character c)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("CharacterManager"), transform);
        CharacterManager m = g.GetComponent<CharacterManager>();
        m.SetCharacter(c);
        charsSquad.Add(m);
    }

    public void WalkToPoint(Vector3 _point)
    {
        agent.SetDestination(_point);

        for (int i = 0; i < charsSquad.Count; i++)
        {
            charsSquad[i].WalkToPoint(_point);
        }
    }
        
}
