using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MainPlayerControl : MonoBehaviour
{
    [Inject] private InputControl inputControl;

    private PlayerDomain domain;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        domain = GetComponent<PlayerDomain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > Globals.PLAYER_UPDATE_COOLDOWN)
        {
            _timer = 0;
            Vector3 destination = transform.position + new Vector3(inputControl.Direction.x, 0, inputControl.Direction.y) * 2;
            domain.WalkToPoint(destination);
        }
        else
        {
            _timer += Time.deltaTime;
        }
        
    }
}
