using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimer : MonoBehaviour
{
    private CharacterManager characterManager;
    private List<CharacterManager> characters = new List<CharacterManager>();
    private CapsuleCollider _collider;
    private bool isOne;
    private int team;

    private void OnEnable()
    {
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
    }

    public void SetData(CharacterManager c, float aggroRadius)
    {
        
        characterManager = c;
        _collider.radius = aggroRadius;
        isOne = true;
        team = c.TeamID;
    }

    public void SetData(List<CharacterManager> c, float radius)
    {
        characters = c;
        _collider.radius = radius;
        isOne = false;
        team = c[0].TeamID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((isOne || characters.Count > 0) && other.gameObject.TryGetComponent(out CharacterManager c) && c.TeamID != team)
        {
            if (isOne)
            {
                characterManager.AddAim(c);
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    characters[i].AddAim(c);
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((isOne || characters.Count > 0) && other.gameObject.TryGetComponent(out CharacterManager c) && c.TeamID != team)
        {
            if (isOne)
            {
                characterManager.RemoveAim(c);
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    characters[i].RemoveAim(c);
                }
            }            
        }
    }
}
