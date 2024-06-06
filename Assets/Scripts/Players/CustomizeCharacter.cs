using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizeCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] level1;
    [SerializeField] private GameObject[] level2;
    [SerializeField] private GameObject[] level3;
    [SerializeField] private GameObject[] level4;
    [SerializeField] private GameObject[] mainMenu;
        
    public void Customize(int level)
    {
        deactivateAll();

        switch(level)
        {
            case 1:
                level1.ToList().ForEach(p => p.SetActive(true));
                break;

            case 2:
                level2.ToList().ForEach(p => p.SetActive(true));
                break;

            case 3:
                level3.ToList().ForEach(p => p.SetActive(true));
                break;

            case 4:
                level4.ToList().ForEach(p => p.SetActive(true));
                break;

            default:
                mainMenu.ToList().ForEach(p => p.SetActive(false));
                break;
        }
    }

    private void deactivateAll()
    {
        level1.ToList().ForEach(p => p.SetActive(false));
        level2.ToList().ForEach(p => p.SetActive(false));
        level3.ToList().ForEach(p => p.SetActive(false));
        level4.ToList().ForEach(p => p.SetActive(false));
        mainMenu.ToList().ForEach(p => p.SetActive(false));
    }
}
