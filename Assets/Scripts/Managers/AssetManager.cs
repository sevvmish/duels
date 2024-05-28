using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject characterManager;
    [SerializeField] private GameObject npcCharacter;

    [Header("Hits and weapons")]
    [SerializeField] private GameObject arrow1;
    [SerializeField] private GameObject meleeSwordSounds;
    [SerializeField] private GameObject bowSounds;

    [Header("Effects")]
    [SerializeField] private GameObject grave01;
    [SerializeField] private GameObject grave02;
    [SerializeField] private GameObject grave03Big;
    [SerializeField] private GameObject consumeGold;

    [Header("UI")]
    [SerializeField] private GameObject playerIndicators;

    //CHARACTER OBJECTS
    private ObjectPool WarriorSamPool;
    private ObjectPool ShooterMikePool;
    private ObjectPool TestBossPool;




    public ObjectPool Grave01Pool => grave01Pool;
    private ObjectPool grave01Pool;
    public ObjectPool Grave02Pool => grave02Pool;
    private ObjectPool grave02Pool;
    public ObjectPool Grave03Pool => grave03Pool;
    private ObjectPool grave03Pool;

    public ObjectPool ConsumeGoldPool => consumeGoldPool;
    private ObjectPool consumeGoldPool;

    public ObjectPool CharacterManagerPool => characterManagerPool;
    private ObjectPool characterManagerPool;

    public ObjectPool NpcCharacterPool => npcCharacterPool;
    private ObjectPool npcCharacterPool;

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
        npcCharacterPool = new ObjectPool(50, npcCharacter, transform);

        arrow1Pool = new ObjectPool(20, arrow1, transform);
        meleeSoundsPool = new ObjectPool(20, meleeSwordSounds, transform);
        bowSoundsPool = new ObjectPool(20, bowSounds, transform);
        
        grave01Pool = new ObjectPool(20, grave01, transform);
        grave02Pool = new ObjectPool(20, grave02, transform);
        grave03Pool = new ObjectPool(20, grave03Big, transform);

        consumeGoldPool = new ObjectPool(10, consumeGold, transform);

        playerIndicatorPool = new ObjectPool(100, playerIndicators, transform);

        //CHARACTER OBJECTS
        WarriorSamPool = new ObjectPool(10, Resources.Load<GameObject>("Characters/WarriorSam"), transform);
        ShooterMikePool = new ObjectPool(10, Resources.Load<GameObject>("Characters/ShooterMike"), transform);
        TestBossPool = new ObjectPool(10, Resources.Load<GameObject>("Characters/TestBoss"), transform);
    }

    public void SetGrave(Vector3 pos, CharacterTypesByCathegory heroType)
    {
        ObjectPool p = default;

        if (heroType == CharacterTypesByCathegory.Squad)
        {
            ObjectPool[] pools = new ObjectPool[] { grave01Pool, grave02Pool };
            p = pools[UnityEngine.Random.Range(0, pools.Length)];
        }
        else if (heroType == CharacterTypesByCathegory.SquadHero)
        {
            p = grave03Pool;
        }


        GameObject g = p.GetObject();
        g.transform.position = pos;
        Transform t = g.transform.GetChild(0);
        t.position = pos + Vector3.up * 10f;
        t.eulerAngles += new Vector3(0, UnityEngine.Random.Range(-15,-25), 0);
        g.SetActive(true);
        t.DOMoveY(pos.y, 0.5f).SetEase(Ease.OutExpo);
    }   

    public IEnumerator returnObjectToPool(ObjectPool pool, GameObject g, float _timer)
    {
        yield return new WaitForSeconds(_timer);
        pool.ReturnObject(g);
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

    public GameObject GetCharacterObject(CharacterTypesByUniqueName _type)
    {
        switch (_type)
        {
            case CharacterTypesByUniqueName.WarriorSam: return WarriorSamPool.GetObject();
            case CharacterTypesByUniqueName.ShooterMike: return ShooterMikePool.GetObject();
            case CharacterTypesByUniqueName.TestBoss: return TestBossPool.GetObject();
        }

        return null;
    }

    public void ReturnCharacterObject(GameObject g, CharacterTypesByUniqueName _type)
    {
        switch (_type)
        {
            case CharacterTypesByUniqueName.WarriorSam: 
                WarriorSamPool.ReturnObject(g);
                break;

            case CharacterTypesByUniqueName.ShooterMike:
                ShooterMikePool.ReturnObject(g);
                break;

            case CharacterTypesByUniqueName.TestBoss:
                TestBossPool.ReturnObject(g);
                break;

        }                
    }
}
