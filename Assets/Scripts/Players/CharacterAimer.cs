using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimer : MonoBehaviour
{
    public List<CharacterManager> Aims { get; private set; }
    private HashSet<CharacterManager> aimCheck = new HashSet<CharacterManager>();

    private CapsuleCollider _collider;
    private int team;
    private bool isInited;

    public void SetData(float aggroRadius, int team)
    {
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
        Aims = new List<CharacterManager> ();

        _collider.radius = aggroRadius;        
        this.team = team;
        isInited = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInited && other.gameObject.TryGetComponent(out CharacterManager c) && c.TeamID != team)
        {
            addAim(c);            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInited && other.gameObject.TryGetComponent(out CharacterManager c) && c.TeamID != team)
        {
            removeAim(c);
        }
    }

    public void addAim(CharacterManager newAim)
    {
        if (aimCheck.Contains(newAim)) return;

        aimCheck.Add(newAim);
        Aims.Add(newAim);
    }

    public void removeAim(CharacterManager newAim)
    {
        if (!aimCheck.Contains(newAim)) return;

        aimCheck.Remove(newAim);
        Aims.Remove(newAim);
    }
}
