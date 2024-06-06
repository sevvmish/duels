using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Zenject;

public class AssetManager : MonoBehaviour
{
    [Inject] private EffectsManager effects;

    [Header("Main")]
    [SerializeField] private GameObject characterManager;
    [SerializeField] private GameObject npcCharacter;
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private GameObject damageDealer;

    [Header("Characters")]
    [SerializeField] private GameObject simpleWarriorWithShield;
    [SerializeField] private GameObject simpleArcher;
    [SerializeField] private GameObject simpleMageBold;
    [SerializeField] private GameObject vikingHero;
    [SerializeField] private GameObject wizardHero;

    [Header("Hits and weapons")]
    [SerializeField] private GameObject arrow1;
    [SerializeField] private GameObject meleeSwordSounds;
    [SerializeField] private GameObject bowSounds;

    [Header("Gameplay Role UI Sprites")]
    [SerializeField] private Sprite meleeDamagerIcon;
    [SerializeField] private Sprite rangedDamagerIcon;
    [SerializeField] private Sprite tankIcon;
    [SerializeField] private Sprite healerIcon;
    [SerializeField] private Sprite magicDamagerIcon;


    [Header("UI")]
    [SerializeField] private GameObject playerIndicators;

    //CHARACTER OBJECTS
    private ObjectPool WarriorSamPool;
    private ObjectPool ShooterMikePool;
    private ObjectPool TestBossPool;
    private ObjectPool VikingHeroPool;
    private ObjectPool WizardHeroPool;
    private ObjectPool PriestSimplePool;

    public ObjectPool CharacterManagerPool => characterManagerPool;
    private ObjectPool characterManagerPool;

    public ObjectPool NpcCharacterPool => npcCharacterPool;
    private ObjectPool npcCharacterPool;

    public ObjectPool GoldCoinPool => goldCoinPool;
    private ObjectPool goldCoinPool;

    public ObjectPool DamageDealerPool => damageDealerPool;
    private ObjectPool damageDealerPool;

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
        goldCoinPool = new ObjectPool(50, goldCoin, transform);
        damageDealerPool = new ObjectPool(50, damageDealer, transform);

        arrow1Pool = new ObjectPool(20, arrow1, transform);
        meleeSoundsPool = new ObjectPool(20, meleeSwordSounds, transform);
        bowSoundsPool = new ObjectPool(20, bowSounds, transform);
        

        playerIndicatorPool = new ObjectPool(100, playerIndicators, transform);

        //CHARACTER OBJECTS
        WarriorSamPool = new ObjectPool(30, simpleWarriorWithShield, transform);
        ShooterMikePool = new ObjectPool(30, simpleArcher, transform);
        TestBossPool = new ObjectPool(30, simpleMageBold, transform);
        VikingHeroPool = new ObjectPool(30, vikingHero, transform);
        PriestSimplePool = new ObjectPool(30, simpleMageBold, transform);
        WizardHeroPool = new ObjectPool(30, wizardHero, transform);
    }
       
    public IEnumerator returnObjectToPool(ObjectPool pool, GameObject g, float _timer)
    {
        yield return new WaitForSeconds(_timer);
        pool.ReturnObject(g);
    }

    public Sprite GetIconGameplayRole(CharacterGameplayRoles _type)
    {
        switch(_type)
        {
            case CharacterGameplayRoles.meleeDamager: return meleeDamagerIcon;
            case CharacterGameplayRoles.rangedDamager: return rangedDamagerIcon;
            case CharacterGameplayRoles.tank: return tankIcon;
            case CharacterGameplayRoles.healer: return healerIcon;
            case CharacterGameplayRoles.magicDamager: return magicDamagerIcon;
        }

        return null;
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
        return GetCharacterObject(_type, 1);
    }

    public GameObject GetCharacterObject(CharacterTypesByUniqueName _type, int level)
    {
        GameObject g = default;

        switch (_type)
        {
            case CharacterTypesByUniqueName.WarriorSam: return WarriorSamPool.GetObject();
            case CharacterTypesByUniqueName.ShooterMike: return ShooterMikePool.GetObject();
            case CharacterTypesByUniqueName.TestBoss: return TestBossPool.GetObject();
            case CharacterTypesByUniqueName.VikingHero: 
                g = VikingHeroPool.GetObject();
                if (g.TryGetComponent(out CustomizeCharacter c))
                {
                    c.Customize(level);
                }
                return g;
            case CharacterTypesByUniqueName.PriestSimpleHuman: return PriestSimplePool.GetObject();
            case CharacterTypesByUniqueName.WizardHero:
                g = WizardHeroPool.GetObject();
                if (g.TryGetComponent(out CustomizeCharacter c1))
                {
                    c1.Customize(level);
                }
                return g;
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

            case CharacterTypesByUniqueName.VikingHero:
                VikingHeroPool.ReturnObject(g);
                break;

            case CharacterTypesByUniqueName.WizardHero:
                WizardHeroPool.ReturnObject(g);
                break;

            case CharacterTypesByUniqueName.PriestSimpleHuman:
                PriestSimplePool.ReturnObject(g);
                break;

        }                
    }
}
