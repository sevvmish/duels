using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChoosingCharacterUI : MonoBehaviour
{
    [Inject] private Sounds sounds;
    [Inject] private AssetManager assets;

    public bool IsActive { get; private set; }
    
    [SerializeField] private RectTransform sidePanel;
    [SerializeField] private TextMeshProUGUI chooseYourSquadText;
    [SerializeField] private GameObject mainBack;
    [SerializeField] private GameObject panelExample;
    [SerializeField] private Transform locationForThreePanels;

    private List<GameObject> panels = new List<GameObject>();
    private List<Character> charactersToChoose = new List<Character>();
    private Action<Character> pressedButtonResult;

    private void Start()
    {
        chooseYourSquadText.text = Globals.Language.ChooseYourSquad;

        sidePanel.DOAnchorPos(new Vector2(800, 0), 0.5f).SetEase(Ease.InSine);
        IsActive = false;
        mainBack.SetActive(false);
        sidePanel.gameObject.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            GameObject g = Instantiate(panelExample, locationForThreePanels);
            g.SetActive(true);
            panels.Add(g);
        }        
    }

    
    public void ActivatePanel(bool isActive) => ActivatePanel(isActive, null, null);
    public void ActivatePanel(bool isActive, List<Character> characters, Action<Character> result)
    {
        if (this.IsActive && !isActive)
        {
            this.IsActive = false;
            sidePanel.DOAnchorPos(new Vector2(800, 0), 0.5f).SetEase(Ease.InSine).OnComplete(() => { sidePanel.gameObject.SetActive(false); });
            mainBack.SetActive(false);
        }
        else if(!this.IsActive && isActive)
        {
            sounds.PlaySound(SoundTypes.boom);
            sidePanel.gameObject.SetActive(true);
            this.IsActive = true;
            mainBack.SetActive(true);
            sidePanel.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.InSine);

            if (characters != null && characters.Count > 0)
            {
                charactersToChoose = characters;

                for (int i = 0; i < charactersToChoose.Count; i++)
                {
                    SetPanelsData(panels[i], charactersToChoose[i]);
                }

                pressedButtonResult = result;
            }
            else
            {
                print("ERROR: no players to choose");
            }
            
        }
    }
    
    private void playerActivated(Character c)
    {        
        pressedButtonResult?.Invoke(c);
        pressedButtonResult = null;
        panels.ForEach(p => p.transform.GetChild(5).GetComponent<Button>().interactable = false);

        for (int i = 0; i < charactersToChoose.Count; i++)
        {
            if (c.CharacterTypeByUniqueName == charactersToChoose[i].CharacterTypeByUniqueName)
            {
                StartCoroutine(playClose(panels[i].transform));
            }
            else
            {
                panels[i].transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);
            }
        }
    }
    private IEnumerator playClose(Transform t)
    {
        sounds.PlaySound(SoundTypes.success2);
        t.GetChild(6).gameObject.SetActive(true);
        t.DOShakeScale(0.3f, 1, 30).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        ActivatePanel(false);
    }

    private void SetPanelsData(GameObject panel, Character c)
    {
        panel.transform.localScale = Vector3.one;

        //name
        panel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = c.CharacterTypeByUniqueName.ToString();

        //gameplay role
        panel.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = assets.GetIconGameplayRole(c.CharacterGameplayRole);

        //character info
        //character unique quality
        panel.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.3f, 0);
        //character icon
        //panel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = 

        //character game data (hp, dps)
        //HP
        panel.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.Health;
        panel.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = c.MaxHP.ToString("f0");
        //DPS
        panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.Damage;
        panel.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = c.DPS.ToString("f0");

        //description
        panel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "J ferhbuf eriuf yiwe fyw fy wyf  wufy wf";

        //activator
        panel.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
        panel.transform.GetChild(5).GetComponent<Button>().interactable = true;
        panel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => 
        { 

            playerActivated(c); 
        });

        //outline
        panel.transform.GetChild(6).gameObject.SetActive(false);
    }
}
