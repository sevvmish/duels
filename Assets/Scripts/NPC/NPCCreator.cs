using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCreator : MonoBehaviour
{
    //[Inject] CharacterManager.Factory factory;
    //[Inject] private GameManager gm;
    
    [SerializeField] private CharacterTypesByUniqueName currentNPS = CharacterTypesByUniqueName.TestBoss;    
    [SerializeField] private int team = -1;

    private AssetManager assets;
    private DynamicUI dynamicUI;

    // Start is called before the first frame update
    void Start()
    {   
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        dynamicUI = GameObject.Find("DynamicCanvas").GetComponent<DynamicUI>();
                
        AddCharacter(new Character(currentNPS, 1));
    }

    public CharacterManager AddCharacter(Character c)
    {
        CharacterManager m = assets.CharacterManagerPool.GetObject().GetComponent<CharacterManager>();
        m.gameObject.SetActive(true);
        m.transform.parent = transform;
        m.transform.localPosition = Vector3.zero;
        m.gameObject.name = c.Name;
        GameObject g = m.SetCharacter(c, team, c.CurrentSpeed, Character.GetCharacterObject(c.CharacterTypes), PlayerTypes.npc, RemoveCharacter);
        m.IsReadyForAction = true;

        g.AddComponent<CharacterAimer>();
        CharacterAimer aimer = g.GetComponent<CharacterAimer>();
        aimer.SetData(c.AggroRadius, team);
        m.PlayerAims = aimer.Aims;

        dynamicUI.AddCharacter(m, false);
        return m;
    }

    public void RemoveCharacter(CharacterManager character)
    {
        StartCoroutine(playRemoveCharacter(character));
    }
    private IEnumerator playRemoveCharacter(CharacterManager character)
    {
        yield return new WaitForSeconds(2);
        assets.CharacterManagerPool.ReturnObject(character.gameObject);
    }

}
