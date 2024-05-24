using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorClient : MonoBehaviour
{
    private RotatorManager _manager;
    
    void Start()
    {
        _manager = GameObject.Find("RotatorManager").GetComponent<RotatorManager>();
        _manager.Add(transform);
    }

}
