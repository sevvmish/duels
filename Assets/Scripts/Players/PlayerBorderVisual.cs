using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBorderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem borderLine;
    [SerializeField] private ParticleSystem borderLight;

    private readonly float baseSize = 5f;
    private float currentSize = 5f;
    private bool isBlue = true;

    // Start is called before the first frame update
    void Start()
    {
        SetSize(currentSize);
    }

    public void SetSize(float size)
    {
        currentSize = size;
        borderLine.gameObject.transform.localScale = Vector3.one / baseSize * size;
        borderLight.gameObject.transform.localScale = Vector3.one / baseSize * size;
    }

    public void SetBusyWithEnemies(bool isBusy)
    {
        if (isBusy && isBlue)
        {
            isBlue = false;
            borderLine.startColor = new Color(1, 0, 0, 1);
            borderLine.Stop();
            borderLine.Play();
            borderLight.startColor = new Color(1, 0, 0, 1);
            borderLight.Stop();
            borderLight.Play();
        }
        else if (!isBusy && !isBlue)
        {
            isBlue = true;

            borderLine.startColor = new Color(0, 1, 1, 1);
            borderLine.Stop();
            borderLine.Play();
            borderLight.startColor = new Color(0, 1, 1, 1);
            borderLight.Stop();
            borderLight.Play();
        }
    }

}
