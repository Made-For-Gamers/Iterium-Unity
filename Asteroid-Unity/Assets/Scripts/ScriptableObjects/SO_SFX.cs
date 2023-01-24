using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SfxList", menuName = "Add SO/Common/SoundFX List")]

public class SO_SFX : ScriptableObject
{
    public List<AudioClip> clips = new List<AudioClip>();

    public AudioClip SelectRandomSound()
    {
        int rnd = Random.Range(0, clips.Count);
        return clips[rnd];
    }
}