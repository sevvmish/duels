using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Debug.Log("UI");
        Container.Bind<MainMenuUI>().FromComponentInHierarchy(true).AsSingle();

        Container.Bind<Musics>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<Sounds>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<ScreenSaver>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<GoldCoinsUI>().FromComponentInHierarchy(true).AsSingle();
        Container.Bind<PlayerRatingUI>().FromComponentInHierarchy(true).AsSingle();
    }
}
