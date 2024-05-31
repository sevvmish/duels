using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DynamicUI : MonoBehaviour
{    
    [SerializeField] private Transform location;

    [SerializeField] private GameObject SignUIPrefab;

    [Inject] private Camera mainCamera;
    [Inject] private MainPlayerControl mainPlayer;
    [Inject] private AssetManager assets;

    //limit fir taking into account
    private float Xlimit = 15;
    private float Zlimit = 10;

    private List<IPlayer> characters = new List<IPlayer>();
    private Dictionary<IPlayer, PlayerIndicatorUI> data = new Dictionary<IPlayer, PlayerIndicatorUI>();

    private List<IPlayer> toUpdate = new List<IPlayer>();
    private float _toUpdatePlayers = 1;
    //

    private void Start()
    {
        if (!Globals.IsMobile)
        {
            Xlimit = 22;
            Zlimit = 17;
        }
        else
        {
            Xlimit = 15;
            Zlimit = 10;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_toUpdatePlayers > 0.3f)
        {
            _toUpdatePlayers = 0;

            Vector3 main = mainPlayer.transform.position + Vector3.forward * 2;

            for (int i = 0; i < characters.Count; i++)
            {
                Vector3 pl = characters[i].PlayerTransform.position;                

                if (Mathf.Abs(pl.x - main.x) < 15 && Mathf.Abs(pl.z - main.z) < 10)
                {
                    if (!toUpdate.Contains(characters[i])) toUpdate.Add(characters[i]);
                }
                else
                {
                    if (toUpdate.Contains(characters[i])) toUpdate.Remove(characters[i]);
                    if (data[characters[i]].gameObject.activeSelf) data[characters[i]].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            _toUpdatePlayers += Time.deltaTime;
        }

        for (int i = 0; i < toUpdate.Count; i++)
        {
            if (!data[toUpdate[i]].gameObject.activeSelf) data[toUpdate[i]].gameObject.SetActive(true);
            Vector3 upV = Vector3.up;

            switch (toUpdate[i].Character.Size)
            {
                case CharacterSized.small:
                    upV *= 2f;
                    break;

                case CharacterSized.medium:
                    upV *= 3f;
                    break;

                case CharacterSized.big:
                    upV *= 4f;
                    break;
            }

            Vector2 v = mainCamera.WorldToScreenPoint(toUpdate[i].PlayerTransform.position + upV);
            data[toUpdate[i]].Rect.anchoredPosition = v;
            data[toUpdate[i]].UpdateHP();
        }


        /*
        for (int i = 0; i < characters.Count; i++)
        {
            Vector3 pl = characters[i].PlayerTransform.position;
            Vector3 main = mainPlayer.transform.position + Vector3.forward*2;

            if (Mathf.Abs(pl.x - main.x) < 15 && Mathf.Abs(pl.z - main.z) < 10)
            {
                if (!data[characters[i]].gameObject.activeSelf) data[characters[i]].gameObject.SetActive(true);
                Vector3 upV = Vector3.up;

                switch (characters[i].Character.Size)
                {
                    case CharacterSized.small:
                        upV *= 2f;
                        break;

                    case CharacterSized.medium:
                        upV *= 3f;
                        break;

                    case CharacterSized.big:
                        upV *= 4f;
                        break;
                }

                Vector2 v = mainCamera.WorldToScreenPoint(pl + upV);
                data[characters[i]].Rect.anchoredPosition = v;
                data[characters[i]].UpdateHP();
            }
            else
            {
                if (data[characters[i]].gameObject.activeSelf) data[characters[i]].gameObject.SetActive(false);
            }
        }*/


    }

    public void GameWin()
    {

    }

    public void AddCharacter(IPlayer c, bool isPlayer)
    {
        characters.Add(c);
        GameObject g = assets.PlayerIndicatorPool.GetObject();
        g.transform.parent = location;
        g.SetActive(false);
        PlayerIndicatorUI pi = g.GetComponent<PlayerIndicatorUI>();
        pi.SetData(isPlayer, c, RemoveCharacter);
        data.Add(c, pi);
    }

    public void RemoveCharacter(IPlayer c)
    {
        characters.Remove(c);
        toUpdate.Remove(c);
        assets.PlayerIndicatorPool.ReturnObject(data[c].gameObject);
        data.Remove(c);
    }

    public void AddSign(Transform aim)
    {

    }
}
