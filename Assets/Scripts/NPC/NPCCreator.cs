using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCreator : MonoBehaviour
{
    public CharacterManager NPC { get; private set; }

    [SerializeField] private CharacterTypesByUniqueName currentNPS = CharacterTypesByUniqueName.TestBoss;
    [SerializeField] private float aggroRadius = 5f;
    [SerializeField] private int team = -1;

    // Start is called before the first frame update
    void Start()
    {
        addCharacter(new Character(currentNPS, 1));

        if (TryGetComponent(out CharacterAimer c))
        {
            
            c.SetData(NPC, aggroRadius);
        }
    }

    private void addCharacter(Character c)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("CharacterManager"), transform);
        CharacterManager m = g.GetComponent<CharacterManager>();
        NPC = m;
        m.SetCharacter(c, team);
        m.SetReadyForAction(false);
    }


}
