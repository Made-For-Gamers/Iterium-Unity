using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [Header("Play GameManager music by index")]
    [SerializeField] private int trackIndex;
    [SerializeField] private bool loop;
    [SerializeField] private bool StopCurrent;
    [SerializeField] private float delay;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(trackIndex, loop,StopCurrent, delay);
    }
}
