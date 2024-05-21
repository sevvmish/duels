using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBorderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem borderLine;
    [SerializeField] private ParticleSystem borderLight;

    private readonly float baseSize = 5f;
    private float currentSize = 5f;

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

}
