using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject characterManager;

    [Header("Hits and weapons")]
    [SerializeField] private GameObject arrow1;
    [SerializeField] private GameObject meleeSwordSounds;
    [SerializeField] private GameObject bowSounds;

    [Header("Effects")]
    [SerializeField] private GameObject grave01;
    [SerializeField] private GameObject consumeGold;

    [Header("UI")]
    [SerializeField] private GameObject playerIndicators;


    public ObjectPool Grave01Pool => grave01Pool;
    private ObjectPool grave01Pool;

    public ObjectPool ConsumeGoldPool => consumeGoldPool;
    private ObjectPool consumeGoldPool;

    public ObjectPool CharacterManagerPool => characterManagerPool;
    private ObjectPool characterManagerPool;

    public ObjectPool Arrow1Pool => arrow1Pool;
    private ObjectPool arrow1Pool;

    public ObjectPool BowSoundsPool => bowSoundsPool;
    private ObjectPool bowSoundsPool;

    public ObjectPool MeleeSoundsPool => meleeSoundsPool;
    private ObjectPool meleeSoundsPool;

    public ObjectPool PlayerIndicatorPool => playerIndicatorPool;
    private ObjectPool playerIndicatorPool;


    private void Awake()
    {
        gameObject.name = "AssetManager";

        characterManagerPool = new ObjectPool(50, characterManager, transform);

        arrow1Pool = new ObjectPool(20, arrow1, transform);
        meleeSoundsPool = new ObjectPool(20, meleeSwordSounds, transform);
        bowSoundsPool = new ObjectPool(20, bowSounds, transform);
        grave01Pool = new ObjectPool(20, grave01, transform);

        consumeGoldPool = new ObjectPool(10, consumeGold, transform);

        playerIndicatorPool = new ObjectPool(100, playerIndicators, transform);
    }

    public void SetGrave(Vector3 pos)
    {        
        GameObject g = Grave01Pool.GetObject();
        g.transform.position = pos;
        Transform t = g.transform.GetChild(0);
        t.position = pos + Vector3.up * 10f;
        t.eulerAngles += new Vector3(0, -20/*UnityEngine.Random.Range(-30,30)*/, 0);
        g.SetActive(true);
        t.DOMoveY(pos.y, 0.5f).SetEase(Ease.OutElastic);
    }   
    
    public void ShowConsumeGold(Vector3 pos)
    {
        StartCoroutine(playEffect(consumeGoldPool, pos, 1));
    }

    private IEnumerator playEffect(ObjectPool pool, Vector3 pos, float _timer)
    {
        GameObject g = pool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);
        yield return new WaitForSeconds(_timer);
        pool.ReturnObject(g);
    }
}
