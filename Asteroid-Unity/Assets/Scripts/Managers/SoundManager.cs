using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Singleton manager to manage sound/music specific data
/// </summary>

public class SoundManager : Singleton<SoundManager>
{
    [Header("Sound Effects")]
    [SerializeField] private SO_SFX asteroidExplosion;

    [Header("Settings")]
    [SerializeField] private int audioNumber = 10;

    [Header("Audio Mixer Channels")]
    [SerializeField] private AudioMixerGroup mixerMaster;
    [SerializeField] private AudioMixerGroup mixerMusic;
    [SerializeField] private AudioMixerGroup mixerUi;
    [SerializeField] private AudioMixerGroup mixerSfx;

    private AudioSource[] audiosourceSfx;

    private void Start()
    {
        audiosourceSfx = new AudioSource[audioNumber];
        for (int i = 0; i < audioNumber; i++)
        {
            audiosourceSfx[i] = gameObject.AddComponent<AudioSource>();
            audiosourceSfx[i].outputAudioMixerGroup = mixerSfx;
            print(i);
        }
    }

    public void PlayAsteroidExplosion()
    {
        AudioSource audio = GetAudioSource();
        audio.clip = asteroidExplosion.SelectRandomSound();
        audio.Play();
    }

    private AudioSource GetAudioSource()
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
