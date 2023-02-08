using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Iterium
{

    /// <summary>
    /// Singleton manager to manage sound/music
    /// </summary>

    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Sound Effects")] //ScriptableObject sfx List
        [SerializeField] private SO_SFX asteroidExplosion;
        [SerializeField] private SO_SFX shipExplosion;
        [SerializeField] private SO_SFX effects;

        [Header("Music")]
        [SerializeField] private SO_SFX music; //ScriptableObject music List

        [Header("Settings")]
        [SerializeField] private int audioSourceNumber = 10; //number of re-usable AudioSources

        [Header("Audio Mixer Channels")]
        [SerializeField] private AudioMixerGroup mixerMaster;
        [SerializeField] private AudioMixerGroup mixerMusic;
        [SerializeField] private AudioMixerGroup mixerSfx;

        private AudioSource[] audiosourceSfx;
        private AudioSource audiosourceMusic;

        private new void Awake()
        {
            //Init SFX AudioSources
            audiosourceSfx = new AudioSource[audioSourceNumber];
            for (int i = 0; i < audioSourceNumber; i++)
            {
                audiosourceSfx[i] = gameObject.AddComponent<AudioSource>();
                audiosourceSfx[i].outputAudioMixerGroup = mixerSfx;
            }

            //Init Music Audiosource
            audiosourceMusic = gameObject.AddComponent<AudioSource>();
            audiosourceMusic.outputAudioMixerGroup = mixerMusic;
        }

        private void Start()
        {
            mixerSfx.audioMixer.SetFloat("Sound", GameManager.Instance.player.EffectsVolume);
            mixerMusic.audioMixer.SetFloat("Music", GameManager.Instance.player.MusicVolume);
        }

        //Asteroid explosions from SO_SFX asset
        public void PlayAsteroidExplosion()
        {
            AudioSource audio = GetAudioSourceSfx();
            if (!audio == false)
            {
                audio.clip = asteroidExplosion.SelectRandomSound();
                audio.Play();
            }
        }

        //Ship explosions from SO_SFX asset
        public void PlayShipExplosion()
        {
            AudioSource audio = GetAudioSourceSfx();
            if (!audio == false)
            {
                audio.clip = shipExplosion.SelectRandomSound();
                audio.Play();
            }
        }

        //Play sound effect by index from SO_SFX asset
        public void PlayEffect(int index)
        {
            AudioSource audio = GetAudioSourceSfx();
            if (!audio == false)
            {
                audio.clip = effects.clips[index];
                audio.Play();
            }
        }

        /// <summary>
        /// Play music from a SO_SFX asset
        /// </summary>
        /// <param name="index">Song item index from the list</param>
        /// <param name="loop">Play the sonf in a loop</param>
        /// <param name="stop">Stop any existing songs from playing</param>
        /// <param name="delay">Play start delay in seconds</param>
        public void PlayMusic(int index, bool loop, bool stop, float delay = 0)
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
        }

        //Return an unused audio source
        private AudioSource GetAudioSourceSfx()
        {
            for (int i = 0; i < audioSourceNumber; i++)
            {
                if (!audiosourceSfx[i].isPlaying)
                {
                    return audiosourceSfx[i];
                }
            }
            return null;
        }
    }
}