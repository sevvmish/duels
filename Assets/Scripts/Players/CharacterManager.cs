using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    public Character Character { get; private set; }
    public float CurrentSpeed { get; private set; }
    public int TeamID { get; private set; }
    public bool IsReadyForAction { get; private set; }
    public bool SetReadyForAction(bool isReady) => IsReadyForAction = isReady;

    private NavMeshAgent agent;
    private AnimationControl _animator;
    private CapsuleCollider _collider;

    private List<CharacterManager> aims = new List<CharacterManager>();
    private HashSet<CharacterManager> aimCheck = new HashSet<CharacterManager>();

    public void SetCharacter(Character c, int team)
    {
        Character = c;
        GameObject g = Instantiate(Character.GetCharacterObject(c.CharacterTypes), transform);
        _animator = g.GetComponent<AnimationControl>();
        _animator.SetData(this);
        TeamID = team;
        _collider.radius = c.DamageRadius / 2f;
    }

    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
    }

    public void WalkToPoint(Vector3 _point) => agent.SetDestination(_point);
       

    private void FixedUpdate()
    {
        CurrentSpeed = agent.velocity.magnitude;
    }

    public void AddAim(CharacterManager newAim)
    {
        if (aimCheck.Contains(newAim)) return;

        aimCheck.Add(newAim);
        aims.Add(newAim);

        /*
        print("======================");
        for (int i = 0; i < aims.Count; i++)
        {
            print(Character.Name + " -> "+ i + " = " + aims[i].Character.Name);
        }
        print("======================");
        */
    }

    public void RemoveAim(CharacterManager newAim)
    {
        if (!aimCheck.Contains(newAim)) return;

        aimCheck.Remove(newAim);
        aims.Remove(newAim);
    }

}
