using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBorderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem borderLine;
    [SerializeField] private ParticleSystem borderLight;

    private readonly float baseSize = 5f;
    private float currentSize = 9f;

    // Start is called before the first frame update
    void Start()
    {
        setSize(currentSize);
    }

    private void setSize(float size)
    {
        currentSize = size;
        borderLine.gameObject.transform.localScale = Vector3.one / baseSize * size;
        borderLight.gameObject.transform.localScale = Vector3.one / baseSize * size;
    }

}
