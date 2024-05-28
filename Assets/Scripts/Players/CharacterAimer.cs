using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CharacterAimer : MonoBehaviour
{
    public List<IPlayer> Aims { get; private set; }

    private CapsuleCollider _collider;
    private int team;
    private bool isInited;
    private float radius;

    private float _timer;

    public void SetData(float aggroRadius, int team)
    {
        radius = aggroRadius;

        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
        _collider.center = Vector3.zero;
        _collider.radius = aggroRadius;
        _collider.height = 0;
        _collider.isTrigger = true;

        Aims = new List<IPlayer> ();
                
        this.team = team;
        isInited = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInited && other.gameObject.layer == 3 && other.gameObject.TryGetComponent(out IPlayer c) && c.TeamID != team && c.Character.IsAlive)
        {            
            addAim(c);            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isInited && other.gameObject.layer == 3 && other.gameObject.TryGetComponent(out IPlayer c) && c.TeamID != team && c.Character.IsAlive)
        {            
            addAim(c);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInited && other.gameObject.layer == 3 && other.gameObject.TryGetComponent(out IPlayer c) && c.TeamID != team)
        {            
            removeAim(c);
        }
    }

    public void addAim(IPlayer newAim)
    {
        if (Aims.Contains(newAim)) return;

        //aimCheck.Add(newAim);
        Aims.Add(newAim);
    }

    public void removeAim(IPlayer newAim)
    {
        if (!Aims.Contains(newAim)) return;

        //aimCheck.Remove(newAim);
        Aims.Remove(newAim);
    }
}
