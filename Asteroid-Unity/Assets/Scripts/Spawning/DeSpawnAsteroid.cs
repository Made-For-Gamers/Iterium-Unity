using UnityEngine;

namespace Iterium
{
    public class DeSpawnAsteroid : MonoBehaviour
    {
        //Remove asteroid after it leaves the screen
        private void OnBecameInvisible()
        {
            if (gameObject.activeSelf)
            {
                AsteroidPooling.asteroidPool.Release(gameObject);
            }
        }
    }
}