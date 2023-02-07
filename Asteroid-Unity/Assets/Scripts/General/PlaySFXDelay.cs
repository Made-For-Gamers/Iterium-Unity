using UnityEngine;

namespace Iterium
{
    //Delay the play of an AdioSource sound

    public class PlaySFXDelay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float delay;
        [SerializeField] private AudioSource audiosource;

        private void Start()
        {
            audiosource.PlayDelayed(delay);
        }
    }
}