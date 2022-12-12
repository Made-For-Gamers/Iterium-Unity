using UnityEngine;

public class DeSpawnAsteroid : MonoBehaviour
{
    //Remove asteroid after it leaves the screen
    private void OnBecameInvisible()
    {
        AsteroidPooling.asteroidPool.Release(this.gameObject);
    }
}
