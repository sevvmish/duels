using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioClip error1;
    [SerializeField] private AudioClip success1;
    [SerializeField] private AudioClip success2;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip beepTick;
    [SerializeField] private AudioClip beepOut;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip boom;
    [SerializeField] private AudioClip coins;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundTypes _type)
    {
        _audio.pitch = 1;

        switch (_type)
        {
            case SoundTypes.error1:
                _audio.Stop();
                _audio.clip = error1;
                _audio.Play();
                break;

            case SoundTypes.success1:
                _audio.Stop();
                _audio.clip = success1;
                _audio.Play();
                break;

            case SoundTypes.success2:
                _audio.Stop();
                _audio.clip = success2;
                _audio.Play();
                break;

            case SoundTypes.win:
                _audio.Stop();
                _audio.clip = win;
                _audio.Play();
                break;

            case SoundTypes.lose:
                _audio.Stop();
                _audio.clip = lose;
                _audio.Play();
                break;

            case SoundTypes.beepTick:
                _audio.Stop();
                _audio.clip = beepTick;
                _audio.Play();
                break;

            case SoundTypes.beepOut:
                _audio.Stop();
                _audio.clip = beepOut;
                _audio.Play();
                break;

            case SoundTypes.click:
                _audio.Stop();
                _audio.clip = click;
                _audio.Play();
                break;

            case SoundTypes.boom:
                _audio.Stop();
                _audio.clip = boom;
                _audio.Play();
                break;

            case SoundTypes.coins:
                _audio.Stop();
                _audio.clip = coins;
                _audio.Play();
                break;
        }
    }

}

public enum SoundTypes
{
    error1,
    success1,
    win,
    lose,
    beepTick,
    beepOut,
    click,
    boom,
    success2,
    coins
}
