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

        levelManager.SetLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenSaver.ShowScreen();
    }

    private void Update()
    {
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
        }
    }

    private void heroChosen(Character c)
    {
        print("Hero " + c.CharacterTypeByUniqueName.ToString() + " is chosen!");
        mainPlayerControl.AddCharacter(c);
    }

    public void TestGM()
    {
        print("Testing GM!!!");
    }
}
