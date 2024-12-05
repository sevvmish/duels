using System.Collections;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioClip error1;
    [SerializeField] private AudioClip success1;
    [SerializeField] private AudioClip success2;
    [SerializeField] private AudioClip beepTick;
    [SerializeField] private AudioClip beepOut;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip cash;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip coin;

    [SerializeField] private AudioClip grab01;
    [SerializeField] private AudioClip grab02;


    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySoundWithDelay(float delay, SoundTypes _type)
    {
        StartCoroutine(playSoundWithDelay(delay, _type));
    }
    private IEnumerator playSoundWithDelay(float delay, SoundTypes _type)
    {
        yield return new WaitForSeconds(delay);

        PlaySound(_type);
    }

    public void InventoryTakeSound()
    {
        _audio.pitch = 1;
        _audio.volume = 0.5f;
        _audio.clip = grab02;
        _audio.Play();
    }

    public void InventoryPutSound()
    {
        _audio.pitch = 1;
        _audio.volume = 0.5f;
        _audio.clip = grab01;
        _audio.Play();
    }

    public void PlayGrabSound()
    {
        int rnd = UnityEngine.Random.Range(0, 2);

        _audio.pitch = 1;
        _audio.volume = 0.5f;

        switch (rnd)
        {
            case 0:
                _audio.clip = grab01;
                _audio.Play();
                break;

            case 1:
                _audio.clip = grab02;
                _audio.Play();
                break;
        }
    }

    public void PlaySound(SoundTypes _type)
    {        
        _audio.pitch = 1;
        _audio.volume = 0.6f;

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
                _audio.volume = 0.7f;
                _audio.clip = click;
                _audio.Play();
                break;

            case SoundTypes.cash:
                _audio.Stop();
                _audio.clip = cash;
                _audio.Play();
                break;

            case SoundTypes.pop:
                _audio.Stop();
                _audio.clip = pop;
                _audio.Play();
                break;

            case SoundTypes.coin:
                _audio.Stop();
                _audio.clip = coin;
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
    success2,
    cash,
    pop,
    coin
}
