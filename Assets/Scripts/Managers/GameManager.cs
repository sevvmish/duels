using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject] private Camera _camera;
    public Transform MainPlayer { get; private set; }
    
    private void Awake()
    {
        Globals.SetQualityLevel();
    }

    private void Start()
    {
        addPlayer(true, Vector3.zero, Vector3.zero, SkinTypes.Viking);
    }

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();
        }
    }
    



    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot, SkinTypes skin)
    {
        //main template
        GameObject g = Instantiate(Resources.Load<GameObject>("PlayerTemplate"));
        g.transform.parent = GameObject.Find("Players").transform;
        g.transform.position = pos;
        g.transform.eulerAngles = rot;

        

        //skin
        GameObject skinObject = Instantiate(Skins.GetSkin(skin), g.transform);
        skinObject.transform.localPosition = Vector3.zero;
        skinObject.transform.localEulerAngles = Vector3.zero;
        skinObject.AddComponent<AnimationControl>();

        g.AddComponent<PlayerControl>();
        g.AddComponent<CameraControl>();
        g.GetComponent<CameraControl>().SetData(_camera);
        g.AddComponent<InputControl>();
        g.GetComponent<InputControl>().SetData(_camera);
        g.AddComponent<AudioListener>();

        g.SetActive(true);
        skinObject.SetActive(true);

        if (isMain)
        {
            MainPlayer = g.transform;
        }

        return g;
    }

}
