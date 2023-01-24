using UnityEngine;

/// <summary>
/// Singleton manager to manage sound/music specific data
/// </summary>

public class SoundManager : Singleton<SoundManager>
{
    [Header("Sound Effects")]
    [SerializeField] private SO_SFX asteroidExplosion;

    [Header("Settings")]
    [SerializeField] private int audioNumber = 10;

    private AudioSource[] audiosource;


    private void Start()
    {
        audiosource = new AudioSource[audioNumber];
        for (int i = 0; i < audioNumber; i++)
        {
            audiosource[i] = gameObject.AddComponent<AudioSource>();
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
            if (!audiosource[i].isPlaying)
            { 
                return audiosource[i];
            }
        }
        return null;
    }
}
