using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MainPlayerControl : MonoBehaviour
{
    [Inject] private InputControl inputControl;

    public PlayerDomain MainDomain { get; private set; }
    

    private PlayerBorderVisual border;
    
    private float _timer = 0;

    private int team = 1;
    private float radius = 5.3f;
    private float maxSpeed = Globals.PLAYER_BASE_MAXSPEED;

    private Vector3 currentDestination, destination;


    // Start is called before the first frame update
    void Start()
    {
        MainDomain = GetComponent<PlayerDomain>();
        MainDomain.IsItMainPlayer = true;
        MainDomain.SetData(team, radius, maxSpeed, PlayerTypes.main_player);

        MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.VikingHero, 1));
        //MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.PriestSimpleHuman, 1));
        //MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.WarriorSam, 1));
        //MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.ShooterMike, 1));
        //MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.WarriorSam, 1));
        //MainDomain.AddCharacter(new Character(CharacterTypesByUniqueName.ShooterMike, 1));
        //domain.AddCharacter(new Character(CharacterTypesByUniqueName.WarriorSam, 1));
        //domain.AddCharacter(new Character(CharacterTypesByUniqueName.ShooterMike, 1));

        AddBorderVisual(MainDomain);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > Globals.PLAYER_UPDATE_JOYSTICK_COOLDOWN)
        {
            _timer = 0;
            
            if (inputControl.Direction != Vector2.zero && MainDomain.IsCanMove)
            {
                destination = transform.position + new Vector3(inputControl.Direction.x, 0, inputControl.Direction.y) * 2;
                MainDomain.WalkToPoint(destination);                
            }

            currentDestination = destination;
        }
        else
        {
            _timer += Time.deltaTime;
        }


        if (Mathf.Abs(inputControl.Direction.x) < 0.15f && Mathf.Abs(inputControl.Direction.y) < 0.15f)
        {
            MainDomain.SetReadyForAction(true);

            if (MainDomain.CharacterAimer.Aims.Count > 0)
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
            MainDomain.SetReadyForAction(false);
            border.SetBusyWithEnemies(false);
        }
    }

    public void AddCharacter(Character c) => MainDomain.AddCharacter(c);

    public void AddBorderVisual(PlayerDomain d)
    {
        if (border != null) return;
        border = Instantiate(Resources.Load<GameObject>("BorderVisual"), d.transform).GetComponent<PlayerBorderVisual>();
        border.SetSize(radius * 2);
    }
}
