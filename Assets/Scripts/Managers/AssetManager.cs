using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject characterManager;

    [Header("UI")]
    [SerializeField] private GameObject playerIndicators;


    public ObjectPool CharacterManagerPool => characterManagerPool;
    private ObjectPool characterManagerPool;

    public ObjectPool PlayerIndicatorPool => playerIndicatorPool;
    private ObjectPool playerIndicatorPool;


    private void Awake()
    {
        gameObject.name = "AssetManager";

        characterManagerPool = new ObjectPool(50, characterManager, transform);
        playerIndicatorPool = new ObjectPool(100, playerIndicators, transform);
    }
}
