using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCreator : MonoBehaviour
{
    //[Inject] CharacterManager.Factory factory;
    //[Inject] private GameManager gm;
    
    [SerializeField] private CharacterTypesByUniqueName currentNPS = CharacterTypesByUniqueName.TestBoss;
    [SerializeField] private float aggroRadius = 3f;
    [SerializeField] private int team = -1;

    private AssetManager assets;
    private PlayerDomain npcDomain;

    // Start is called before the first frame update
    void Start()
    {   
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();

        npcDomain = GetComponent<PlayerDomain>();
        npcDomain.SetData(team, aggroRadius, PlayerTypes.npc);
        npcDomain.AddCharacter(new Character(currentNPS, 1));
    }

}
