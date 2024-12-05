using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {        
        if (Globals.MainPlayerData == null)
        {

            Globals.MainPlayerData = new PlayerData();
            Globals.IsMobile = Globals.IsMobileChecker();
            Globals.IsSoundOn = true;
            Globals.IsMusicOn = true;
            Globals.IsInitiated = true;

            Globals.MainPlayerData.T = 50000;
            Globals.MainPlayerData.D = 0;

            if (Globals.IsMobile)
            {
                Globals.MainPlayerData.Zoom = 50;
            }
            else
            {
                Globals.MainPlayerData.Zoom = 55;
            }

            //Globals.IsLowFPS = true;

            Globals.MainPlayerData.Inv = new int[32, 3] { { 0, 2, 1 }, {1, 5, 1 }, { 2, 1, 10 }, { 3, 0, 0 }, { 4, 6, 1 }, { 5, 0, 0 }, { 6, 0, 0 }, { 7, 0, 0 }, { 8, 0, 0 }, { 9, 0, 0 }, { 10, 0, 0 }, { 11, 0, 0 }, { 12, 0, 0 }, { 13, 0, 0 }, { 14, 0, 0 }, { 15, 0, 0 }, { 16, 0, 0 }, { 17, 0, 0 }, { 18, 0, 0 }, { 19, 0, 0 }, { 20, 0, 0 }, { 21, 0, 0 }, { 22, 0, 0 }, { 23, 0, 0 }, { 24, 0, 0 }, { 25, 0, 0 }, { 26, 0, 0 }, { 27, 0, 0 }, { 28, 0, 0 }, { 29, 0, 0 }, { 30, 0, 0 }, { 31, 0, 0 }, };

            Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
        }

        /*
        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();

        builder.RegisterComponentInHierarchy<AssetManager>();
        builder.RegisterComponentInHierarchy<ItemManager>();

        builder.RegisterComponentInHierarchy<EffectsManager>();
        builder.RegisterComponentInHierarchy<Joystick>();        
        builder.RegisterComponentInHierarchy<FPSController>();

        builder.RegisterComponentInHierarchy<AimInformerUI>();
        builder.RegisterComponentInHierarchy<CharacterPanelUI>();
        builder.RegisterComponentInHierarchy<ShowDPSUI>();
        

        PlayerControl g = addPlayer(true, Vector3.zero, Vector3.zero, builder).GetComponent<PlayerControl>();
        builder.RegisterComponentInHierarchy<Inventory>();
        builder.RegisterComponentInHierarchy<PlayerControl>();        
        builder.RegisterComponentInHierarchy<FOVControl>();
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<CameraControl>();
        
        builder.RegisterComponentInHierarchy<HitControl>();

        //UI        
        builder.RegisterComponentInHierarchy<GameplayUI>();
        builder.RegisterComponentInHierarchy<InventoryUI>();
        builder.RegisterComponentInHierarchy<ScreenCenterCursor>();

        //Env
        builder.RegisterComponentInHierarchy<WorldGenerator>();        
        builder.RegisterComponentInHierarchy<NatureGenerator>();
        builder.RegisterComponentInHierarchy<TerrainGenerator>();

        builder.RegisterComponentInHierarchy<DayTimeCycle>();
        builder.RegisterComponentInHierarchy<InputControl>();*/

        builder.RegisterComponentInHierarchy<Camera>();
        //builder.RegisterComponentInHierarchy<Joystick>();

        builder.RegisterComponentInHierarchy<GameManager>();
        //builder.RegisterComponentInHierarchy<CameraControl>();


    }

    
}
