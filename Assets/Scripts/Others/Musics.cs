using System.Collections;
using UnityEngine;
using VContainer;

public class Musics : MonoBehaviour
{
    
    [SerializeField] private AudioClip ForestDay;
    [SerializeField] private AudioClip ForestNight;

    private AudioSource _audio;
    private bool isAmbient;
    private bool isDay;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }


    public void PlayMusic(MusicTypes _type)
    {
        _audio.pitch = 1;
        _audio.loop = true;
        _audio.volume = 0.3f;

        switch (_type)
        {
            
            case MusicTypes.forestDay:                
                _audio.Stop();
                _audio.pitch = 0.8f;
                _audio.volume = 0.4f;
                _audio.clip = ForestDay;
                _audio.Play();
                break;

            case MusicTypes.forestNight:
                _audio.Stop();
                _audio.pitch = 0.8f;
                _audio.volume = 0.5f;
                _audio.clip = ForestNight;
                _audio.Play();
                break;

        }
    }

    public void StopAll()
    {
        _audio.Stop();
    }

    public void StartMusic()
    {
        isAmbient = true;
        isDay = true;
        PlayMusic(MusicTypes.forestDay);
        StartCoroutine(playMusicGradient());
     
    }
    
    private IEnumerator playMusicGradient()
    {
        
        float volume = _audio.volume;

        float delta = volume / 15f;
                
        _audio.volume = 0;

        for (int i = 0; i < 15; i++)
        {
            _audio.volume += delta;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}

public enum MusicTypes
{
    forestDay,
    forestNight
}
