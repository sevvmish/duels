using UnityEngine;

public class Musics : MonoBehaviour
{
    [SerializeField] private AudioClip forest;
    [SerializeField] private AudioClip intro;

    private AudioSource _audio;

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
            case MusicTypes.forest:
                _audio.Stop();
                _audio.clip = forest;
                _audio.Play();
                break;

            case MusicTypes.intro:
                _audio.volume = 0.6f;
                _audio.loop = false;
                _audio.Stop();
                _audio.clip = intro;
                _audio.Play();
                break;
        }
    }

    public void StopAll()
    {
        _audio.Stop();
    }

    public void ContinuePlaying()
    {
        PlayMusic(MusicTypes.forest);
    }
}

public enum MusicTypes
{
    intro,
    forest
}
