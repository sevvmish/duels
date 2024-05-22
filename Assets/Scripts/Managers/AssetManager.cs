using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private GameObject characterManager;

    public ObjectPool CharacterManagerPool { get 
        {
            if (characterManagerPool == null)
            {
                characterManagerPool = new ObjectPool(20, characterManager, transform);
            }

            return characterManagerPool;
        } 
    }

    private ObjectPool characterManagerPool;


    private void Awake()
    {
        gameObject.name = "AssetManager";
    }
}
