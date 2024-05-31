using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EffectsManager : MonoBehaviour
{
    [Inject] private AssetManager assets;

    [Header("Effects")]
    [SerializeField] private GameObject grave01;
    [SerializeField] private GameObject grave02;
    [SerializeField] private GameObject grave03Big;
    [SerializeField] private GameObject npcDeathEffect;
    [SerializeField] private GameObject consumeGoldEffect;
    [SerializeField] private GameObject getNewPlayerEffect;
    [SerializeField] private GameObject healEffect;

    public ObjectPool Grave01Pool => grave01Pool;
    private ObjectPool grave01Pool;
    public ObjectPool Grave02Pool => grave02Pool;
    private ObjectPool grave02Pool;
    public ObjectPool Grave03Pool => grave03Pool;
    private ObjectPool grave03Pool;

    public ObjectPool HealEffectPool => healEffectPool;
    private ObjectPool healEffectPool;

    public ObjectPool NpcDeathEffectPool => npcDeathEffectPool;
    private ObjectPool npcDeathEffectPool;

    public ObjectPool GetNewPlayerEffectPool => getNewPlayerEffectPool;
    private ObjectPool getNewPlayerEffectPool;

    public ObjectPool ConsumeGoldEffectPool => consumeGoldEffectPool;
    private ObjectPool consumeGoldEffectPool;

    private void Awake()
    {
        gameObject.name = "EffectsManager";

        grave01Pool = new ObjectPool(20, grave01, transform);
        grave02Pool = new ObjectPool(20, grave02, transform);
        grave03Pool = new ObjectPool(20, grave03Big, transform);
        healEffectPool = new ObjectPool(20, healEffect, transform);
        npcDeathEffectPool = new ObjectPool(20, npcDeathEffect, transform);
        getNewPlayerEffectPool = new ObjectPool(10, getNewPlayerEffect, transform);

        consumeGoldEffectPool = new ObjectPool(10, consumeGoldEffect, transform);
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
        t.eulerAngles += new Vector3(0, UnityEngine.Random.Range(-5, -30), 0);
        g.SetActive(true);
        t.DOMoveY(pos.y, 0.5f).SetEase(Ease.OutExpo);
    }

    public void SetNewPlayerSpawnEffect(Transform pos)
    {
        StartCoroutine(playEffect(getNewPlayerEffectPool, pos, 1.5f, 0.05f));                
    }

    public void SetNPCDeathEffect(Vector3 pos)
    {
        StartCoroutine(playEffect(npcDeathEffectPool, pos, 1.5f, 0));
    }

    public void SpawnGoldCoinsReward(int amount, Vector3 center)
    {
        for (int i = 0; i < amount; i++)
        {            
            float deltaX = UnityEngine.Random.Range(-amount / 2f, amount / 2f); 
            float deltaZ = UnityEngine.Random.Range(-amount / 2f, amount / 2f);
            Vector3 pos = new Vector3(center.x + deltaX, center.y, center.z + deltaZ);

            GameObject g = assets.GoldCoinPool.GetObject();
            g.transform.position = center;

            if (g.TryGetComponent(out BoxCollider b))
            {
                b.enabled = false;
            }

            g.SetActive(true);
            StartCoroutine(playSpawnGoldCoins(g.transform, center, pos));
        }
    }
    private IEnumerator playSpawnGoldCoins(Transform t, Vector3 center, Vector3 pos)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(t.DOMove(pos, 0.5f).SetEase(Ease.Linear));
        seq.Join(t.DOMoveY(1, 0.2f).SetEase(Ease.OutSine));

        yield return new WaitForSeconds(0.2f);

        seq.Join(t.DOMoveY(0, 0.3f).SetEase(Ease.OutBounce));

        yield return new WaitForSeconds(0.3f);

        if (t.TryGetComponent(out BoxCollider b))
        {
            b.enabled = true;
        }
    }

    public void ConsumeGold(GameObject g)
    {
        assets.GoldCoinPool.ReturnObject(g);        
        Vector3 pos = g.transform.position + Vector3.up * 0.5f;
        StartCoroutine(playEffect(consumeGoldEffectPool, pos, 1, 0));
    }

    private IEnumerator playEffect(ObjectPool pool, Vector3 pos, float _timer, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject g = pool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);
        yield return new WaitForSeconds(_timer);
        pool.ReturnObject(g);
    }

    private IEnumerator playEffect(ObjectPool pool, Transform pos, float _timer, float delay)
    {
        WaitForSeconds w = new WaitForSeconds(0.02f);
        GameObject g = pool.GetObject();

        yield return new WaitForSeconds(delay);
        
        g.SetActive(true);

        for (float i = 0; i < _timer; i+=0.02f)
        {
            g.transform.position = pos.position;
            yield return w;
        }
                
        pool.ReturnObject(g);
    }
}
