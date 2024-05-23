using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private Transform location;
    [Inject] private Camera mainCamera;

    private List<CharacterManager> characters = new List<CharacterManager>();
    private Dictionary<CharacterManager, RectTransform> data = new Dictionary<CharacterManager, RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (CharacterManager c in data.Keys)
        {
            Vector2 v = mainCamera.WorldToScreenPoint(c.transform.position);
            data[c].anchoredPosition = v;
        }
    }

    public void GameWin()
    {

    }

    public void AddCharacter(CharacterManager c)
    {
        characters.Add(c);
        GameObject g = Instantiate(pointer, location);
        g.SetActive(true);
        data.Add(c, g.GetComponent<RectTransform>());
    }
}
