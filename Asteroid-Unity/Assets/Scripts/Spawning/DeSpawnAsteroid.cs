using UnityEngine;

public class DeSpawnAsteroid : MonoBehaviour
{
    //Remove asteroid after it leaves the screen
    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            AsteroidPooling.asteroidPool.Release(this.gameObject);
        }
    }
}