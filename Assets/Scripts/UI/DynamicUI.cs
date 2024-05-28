using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DynamicUI : MonoBehaviour
{    
    [SerializeField] private Transform location;
    
    [Inject] private Camera mainCamera;
    [Inject] private MainPlayerControl mainPlayer;
    [Inject] private AssetManager assets;

    private List<IPlayer> characters = new List<IPlayer>();
    private Dictionary<IPlayer, PlayerIndicatorUI> data = new Dictionary<IPlayer, PlayerIndicatorUI>();


    // Update is called once per frame
    void Update()
    {
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
        }


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
        assets.PlayerIndicatorPool.ReturnObject(data[c].gameObject);
        data.Remove(c);
    }
}
