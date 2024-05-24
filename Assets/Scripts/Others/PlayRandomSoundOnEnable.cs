using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float volume = 0.6f;
    [SerializeField] private bool playOnWake = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private float delay = 0;

    private AudioSource _audio;
    private void OnEnable()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = volume;
        _audio.playOnAwake = playOnWake;
        _audio.loop = loop;
        _audio.clip = clips[UnityEngine.Random.Range(0, clips.Length)];

        if (delay > 0)
        {
            StartCoroutine(playSound());
        }
        else
        {
            _audio.Play();
        }

        
    }

    private IEnumerator playSound()
    {
        yield return new WaitForSeconds(delay);
        _audio.Play();
    }
}
