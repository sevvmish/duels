using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MainPlayerControl : MonoBehaviour
{
    [Inject] private InputControl inputControl;

    private PlayerBorderVisual border;
    private PlayerDomain domain;
    private float _timer = 0;

    private int team = 1;
    private float radius = 4.5f;

    private Vector3 currentDestination, destination;


    // Start is called before the first frame update
    void Start()
    {
        domain = GetComponent<PlayerDomain>();
        domain.SetData(team, radius, PlayerTypes.main_player);
        domain.AddCharacter(new Character(CharacterTypesByUniqueName.WarriorSam, 1));
        domain.AddCharacter(new Character(CharacterTypesByUniqueName.ShooterMike, 1));

        AddBorderVisual(domain);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > Globals.PLAYER_UPDATE_COOLDOWN)
        {
            _timer = 0;
            
            if (inputControl.Direction != Vector2.zero)
            {
                destination = transform.position + new Vector3(inputControl.Direction.x, 0, inputControl.Direction.y) * 2;
                domain.WalkToPoint(destination);                
            }

            currentDestination = destination;
        }
        else
        {
            _timer += Time.deltaTime;
        }


        if (inputControl.Direction == Vector2.zero)
        {
            domain.SetReadyForAction(true);

            if (domain.PlayerAims.Count > 0)
            {
                border.SetBusyWithEnemies(true);
            }
            else
            {
                border.SetBusyWithEnemies(false);
            }
        }
        else
        {
            domain.SetReadyForAction(false);
            border.SetBusyWithEnemies(false);
        }
    }

    public void AddBorderVisual(PlayerDomain d)
    {
        if (border != null) return;
        border = Instantiate(Resources.Load<GameObject>("BorderVisual"), d.transform).GetComponent<PlayerBorderVisual>();
        border.SetSize(radius * 2);
    }
}
