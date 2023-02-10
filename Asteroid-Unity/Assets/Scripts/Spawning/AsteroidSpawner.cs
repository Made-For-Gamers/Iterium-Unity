using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{
    // Spawning of random asteroids at set intervals, random rotation and speed, targets an object for direction

    public class AsteroidSpawner : MonoBehaviour
    {
        [Header("Asteroid Spawning")]
        [SerializeField] private float spawnTime;
        [SerializeField] private float speedMin;
        [SerializeField] private float speedMax;
        [SerializeField] private bool spawnOnceOnly;
        [SerializeField] private GameObject target; //asteroids move towards this scene target

        private float speed;

        private void Start()
        {
            if (!spawnOnceOnly)
            {
                InvokeRepeating("SpawnAsteroid", 0, spawnTime);
            }
            else
            {
                SpawnAsteroid();
            }
        }

        //Spawn asteroid
        private GameObject SpawnAsteroid()
        {
            GameObject spawnedAsteroid = AsteroidPooling.asteroidPool.Get();

            //Position, scale and speed
            spawnedAsteroid.transform.position = transform.position;
            spawnedAsteroid.transform.localScale = Vector3.one;
            speed = Random.Range(speedMin, speedMax + 1);

            //Look and move towards target
            spawnedAsteroid.transform.LookAt(target.transform);
            spawnedAsteroid.GetComponent<Rigidbody>().velocity = spawnedAsteroid.transform.forward * speed;

            //Random rotation
            spawnedAsteroid.transform.rotation = Random.rotation;
            return spawnedAsteroid;
        }
    }
}