using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMoverUI : MonoBehaviour
{
    private RectTransform rect;
    private readonly float delta = 10;
    private readonly float deltaTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        StartCoroutine(play());
    }
    private IEnumerator play()
    {
        while (true)
        {
            rect.DOAnchorPos(Vector2.one * UnityEngine.Random.Range(-delta, delta), deltaTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(deltaTime);

        }
    }
}
