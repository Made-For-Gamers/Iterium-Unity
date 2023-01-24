using System.Collections;
using UnityEngine;
using TMPro;

public class PlaySFXList : MonoBehaviour
{
    /// <summary>
    /// Play a sound using a List<> of songs from a SO_SFX ScriptableObject
    /// </summary>
    [Header("SO Playlist")]
    [SerializeField] private SO_SFX clips;

    [Space(10)]
    [Header("Settings")]
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool loop;
    [SerializeField] private float delayTime;
    [Space(10)]
    [Header("Clip to play")]
    [SerializeField] private bool randomClip;
    [Header("OR")]
    [SerializeField] private int clipIndex = 0;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (playOnStart)
        {
            PlayClip(delayTime);
        }
    }

    public void PlayClip(float delay)
    {
        if (randomClip)
        {
            audioSource.clip = clips.SelectRandomSound();
        }
        else
        {
            audioSource.clip = clips.clips[clipIndex];
        }

        if (delay > 0)
        {
            audioSource.PlayDelayed(delay);
        }
        else
        {
            audioSource.Play();
        }

        if (loop)
        {
            StartCoroutine(Loop());
        }
    }

    public void NextClip(float delay)
    {
        clipIndex++;
        if (clipIndex >= clips.clips.Count)
        {
            clipIndex = 0;
        }
        PlayClip(0);

    }

    public void PreviousClip(float delay)
    {
        clipIndex--;
        if (clipIndex < 0)
        {
            clipIndex = clips.clips.Count - 1;
        }
        PlayClip(0);
    }

    public void StopClip(float delay)
    {
        audioSource.Stop();
    }

    IEnumerator Loop()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(clips.clips[clipIndex].length);
        if (audioSource.isPlaying)
        {
            PlayClip(0);
        }
    }
}
