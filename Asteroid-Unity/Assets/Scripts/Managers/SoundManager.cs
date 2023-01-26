using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Singleton manager to manage sound/music specific data
/// </summary>

public class SoundManager : Singleton<SoundManager>
{
    [Header("Sound Effects")]
    [SerializeField] private SO_SFX asteroidExplosion;
    [SerializeField] private SO_SFX shipExplosion;
    [SerializeField] private SO_SFX effects;

    [Header("Music")]
    [SerializeField] private SO_SFX music;

    [Header("Settings")]
    [SerializeField] private int audioNumber = 10;

    [Header("Audio Mixer Channels")]
    [SerializeField] private AudioMixerGroup mixerMaster;
    [SerializeField] private AudioMixerGroup mixerMusic;
    [SerializeField] private AudioMixerGroup mixerUi;
    [SerializeField] private AudioMixerGroup mixerSfx;

    private AudioSource[] audiosourceSfx;
    private AudioSource audiosourceMusic;

    private new void Awake()
    {
        //Init SFX Audiosource
        audiosourceSfx = new AudioSource[audioNumber];
        for (int i = 0; i < audioNumber; i++)
        {
            audiosourceSfx[i] = gameObject.AddComponent<AudioSource>();
            audiosourceSfx[i].outputAudioMixerGroup = mixerSfx;
        }

        //Init Music Audiosource
        audiosourceMusic = gameObject.AddComponent<AudioSource>();
        audiosourceMusic.outputAudioMixerGroup = mixerMusic;
    }

    public void PlayAsteroidExplosion()
    {
        AudioSource audio = GetAudioSourceSfx();
        audio.clip = asteroidExplosion.SelectRandomSound();
        audio.Play();
    }

    public void PlayShipExplosion()
    {
        AudioSource audio = GetAudioSourceSfx();
        audio.clip = shipExplosion.SelectRandomSound();
        audio.Play();
    }

    public void PlayEffect()
    {
        AudioSource audio = GetAudioSourceSfx();
        audio.clip = effects.SelectRandomSound();
        audio.Play();
    }

    public void PlayMusic(int index, bool loop,bool stop, float delay = 0)
    {
        if (stop)
        {
            audiosourceMusic.Stop();
        }
        if (!audiosourceMusic.isPlaying)
        {
            audiosourceMusic.clip = music.clips[index];
            if (loop)
            {
                audiosourceMusic.loop = true;
            }
            else
            {
                audiosourceMusic.loop = false;
            }
            audiosourceMusic.PlayDelayed(delay);
        }
        else
        {
            print("A song is already playing.");
        }
    }

    private AudioSource GetAudioSourceSfx()
    {
        for (int i = 0; i < audioNumber; i++)
        {
            if (!audiosourceSfx[i].isPlaying)
            {
                return audiosourceSfx[i];
            }
        }
        return null;
    }
}
