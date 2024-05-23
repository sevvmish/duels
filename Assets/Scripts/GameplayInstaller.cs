using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    
    public override void InstallBindings()
    {
#if UNITY_EDITOR         
        Globals.MainPlayerData = new PlayerData();
        Globals.IsInitiated = true;
        Globals.IsMobile = true;
        Globals.IsSoundOn = true;
        Globals.IsMusicOn = true;
        Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
        Globals.CurrentLevel = Globals.MainPlayerData.Level;
#endif    

        Debug.Log("gameplay");

        Container.Bind<AssetManager>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<Joystick>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<LevelManager>().FromComponentInHierarchy(true).AsSingle();
        
        Container.Bind<GameManager>().FromComponentInHierarchy(true).AsSingle();        
        Container.Bind<InputControl>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<MainPlayerControl>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<Camera>().FromComponentInHierarchy(true).AsSingle();        
        Container.Bind<CameraControl>().FromComponentInHierarchy(true).AsSingle();
               

        Container.Bind<Musics>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<Sounds>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<ScreenSaver>().FromComponentInHierarchy(true).AsSingle();
                
        Container.Bind<PlayerDomain>().FromComponentInHierarchy(true).AsTransient();
        Container.Bind<GameplayUI>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<DynamicUI>().FromComponentInHierarchy(true).AsSingle();
    }
}
