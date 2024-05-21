using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    public static ScreenSaver Instance { get; private set; }

    [SerializeField] private RectTransform screen1;
    [SerializeField] private RectTransform screen2;

    private readonly float additionalWait = 0.3f;

    void Awake()
    {        
        screen1.anchoredPosition = new Vector2(0, 1000);
        screen1.gameObject.SetActive(true);
        screen2.anchoredPosition = new Vector2(0, -1000);
        screen2.gameObject.SetActive(true);
    }

    public void HideScreen()
    {
        screen1.gameObject.SetActive(true);
        screen1.anchoredPosition = new Vector2(0, 2000);
        screen1.DOAnchorPos3D(new Vector2(0, 1000), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.InOutBounce);

        screen2.gameObject.SetActive(true);
        screen2.anchoredPosition = new Vector2(0, -2000);
        screen2.DOAnchorPos3D(new Vector2(0, -1000), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.InOutBounce);
    }

    public void FastShowScreen()
    {        
        StartCoroutine(deactivateAfter(0));
    }

    public void ShowScreen()
    {        
        screen1.gameObject.SetActive(true);
        screen1.anchoredPosition = new Vector2(0, 1000);
        screen1.DOAnchorPos3D(new Vector2(0, 2000), Globals.SCREEN_SAVER_AWAIT/1.5f).SetEase(Ease.InSine);

        screen2.gameObject.SetActive(true);
        screen2.anchoredPosition = new Vector2(0, -1000);
        screen2.DOAnchorPos3D(new Vector2(0, -2000), Globals.SCREEN_SAVER_AWAIT/1.5f).SetEase(Ease.InSine);

        StartCoroutine(deactivateAfter(Globals.SCREEN_SAVER_AWAIT));
    }

    private IEnumerator deactivateAfter(float secs)
    {
        yield return new WaitForSeconds(secs);
        screen1.gameObject.SetActive(false);
        screen2.gameObject.SetActive(false);
    }


}
