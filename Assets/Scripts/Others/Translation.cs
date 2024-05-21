using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    //character quality level
    public string Common;
    public string Improved;
    public string Unique;
    public string Rare;
    public string Legendary;


    public Translation() { }
}
