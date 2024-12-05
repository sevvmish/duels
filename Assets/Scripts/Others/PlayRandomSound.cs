using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource _audio;

    private void OnEnable()
    {
        _audio.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        _audio.Stop();
        _audio.Play();
    }
}
