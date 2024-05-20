using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    private void OnEnable()
    {   
        surface.BuildNavMesh();
        NavMeshData data = surface.navMeshData;
        surface.UpdateNavMesh(data);
    }
}
