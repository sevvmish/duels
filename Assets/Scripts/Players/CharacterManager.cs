using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    public Character Character { get; private set; }
    public float CurrentSpeed { get; private set; }

    private NavMeshAgent agent;
    private AnimationControl _animator;
    
    public void SetCharacter(Character c)
    {
        Character = c;
        GameObject g = Instantiate(Character.GetCharacterObject(c.CharacterTypes), transform);
        _animator = g.GetComponent<AnimationControl>();
        _animator.SetData(this);
    }

    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    public void WalkToPoint(Vector3 _point) => agent.SetDestination(_point);
       

    private void FixedUpdate()
    {
        CurrentSpeed = agent.velocity.magnitude;
    }

}
