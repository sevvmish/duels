using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{   
    public string L;
    public int M;
    public int S;
    public int Mus;

    public bool IsLowFPS;
    public int Rating;
    public bool AdvOff;

    public int Gold;


    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        
        Rating = 1;
        Gold = 0;

        IsLowFPS = false;        
        AdvOff = false;

        Debug.Log("created PlayerData instance");
    }


}
