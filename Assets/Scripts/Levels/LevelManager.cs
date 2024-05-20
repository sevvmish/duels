using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel { get; private set; }

    [SerializeField] private Level[] levels;
    [SerializeField] private Transform location;


    public void SetLevel()
    {
        CurrentLevel = Instantiate(levels[0], location);
        CurrentLevel.transform.position = Vector3.zero;
        CurrentLevel.transform.eulerAngles = Vector3.zero;
        CurrentLevel.gameObject.SetActive(true);
    }
}
