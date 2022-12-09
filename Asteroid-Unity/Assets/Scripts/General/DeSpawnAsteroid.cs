using UnityEngine;

public class DeSpawnAsteroid : MonoBehaviour
{
    //Remove asteroid after it leaves the screen
    private void OnBecameInvisible()
    {
        if (AsteroidSpawner.isAsteroidPooling)
        {
            AsteroidSpawner.asteroidPool.Release(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
