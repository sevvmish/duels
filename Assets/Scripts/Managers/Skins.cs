using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour 
{
    public static GameObject GetSkin(SkinTypes skin)
    {
        switch(skin)
        {
            case SkinTypes.None:
                return Resources.Load<GameObject>("PlayerTemplate");

            case SkinTypes.Barbarian:
                return Resources.Load<GameObject>("PlayerTemplate");

            case SkinTypes.Viking:
                return Resources.Load<GameObject>("Skins/viking");
        }

        return null;
    }
}

public enum SkinTypes
{
    None,
    Barbarian,
    Viking
}
