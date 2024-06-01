using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private ScreenSaver screenSaver;
    [Inject] private LevelManager levelManager;
    [Inject] private ChoosingCharacterUI chooseCharacter;
    [Inject] private MainPlayerControl mainPlayerControl;

    public float SecondsLeftToEndGame { get; private set; }
    public bool IsGameStarted { get; private set; }
    public bool IsGameEnded { get; private set; }

    private bool isFirstPlayerChosen;


    private void Awake()
    {
        if (Globals.IsMobile)
        {
            QualitySettings.antiAliasing = 2;

            if (Globals.IsLowFPS)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
            }
        }
        else
        {
            QualitySettings.antiAliasing = 4;
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }

        SecondsLeftToEndGame = 4*60;
        levelManager.SetLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenSaver.ShowScreen();
    }

    private void Update()
    {
        //timer
        if (SecondsLeftToEndGame >= 0) SecondsLeftToEndGame -= Time.deltaTime;

        //first player for main player
        if (!isFirstPlayerChosen && IsGameStarted && !IsGameEnded && mainPlayerControl.MainDomain.PlayerSquadAmount > 0)
        {
            isFirstPlayerChosen = true;
        }
        

        //WIN OR LOSE
        if (isFirstPlayerChosen && IsGameStarted && !IsGameEnded && mainPlayerControl.MainDomain.PlayerSquadAmount > 0)
        {
            gameLose();
        }
        else if (SecondsLeftToEndGame <= 0 && IsGameStarted && !IsGameEnded)
        {
            gameEnded();
        }

        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (chooseCharacter.IsActive)
            {
                chooseCharacter.ActivatePanel(false);
            }
            else
            {
                chooseCharacter.ActivatePanel(true, new List<Character>() {new Character(CharacterTypesByUniqueName.WarriorSam, 1),
                                                                           new Character(CharacterTypesByUniqueName.ShooterMike, 1),
                                                                           new Character(CharacterTypesByUniqueName.TestBoss, 1), }, heroChosen);
            }
        }*/
    }

    public void TurnOnChoseCharacterProcess(bool isMainPlayer, SpawnPortal portal)
    {
        portal.StartCooldown();

        if (isMainPlayer)
        {   
            chooseCharacter.ActivatePanel(true, new List<Character>() {new Character(CharacterTypesByUniqueName.WarriorSam, 1),
                                                                           new Character(CharacterTypesByUniqueName.ShooterMike, 1),
                                                                           new Character(CharacterTypesByUniqueName.PriestSimpleHuman, 1), }, heroChosen);
        }
        else
        {

        }
    }

    private void gameLose()
    {
        IsGameEnded = true;
    }

    private void gameEnded()
    {
        IsGameEnded = true;
    }

    private void heroChosen(Character c)
    {
        print("Hero " + c.CharacterTypeByUniqueName.ToString() + " is chosen!");
        mainPlayerControl.AddCharacter(c);
    }
        
}
