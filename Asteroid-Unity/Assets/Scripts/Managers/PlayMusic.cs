using UnityEngine;

namespace Iterium
{
    //Play music track via the SoundManager
    //* Loop
    //* Stop current sound
    //* Delay
    //* Play by index

    public class PlayMusic : MonoBehaviour
    {
        [Header("Music by index")]
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