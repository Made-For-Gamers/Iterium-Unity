using UnityEngine;

namespace Iterium
{
    public class PlayMusic : MonoBehaviour
    {
        [Header("Play music by index")]
        [SerializeField] private int trackIndex;
        [SerializeField] private bool loop;
        [SerializeField] private bool StopCurrent;
        [SerializeField] private float delay;

        private void Start()
        {
            SoundManager.Instance.PlayMusic(trackIndex, loop, StopCurrent, delay);
        }
    }
}