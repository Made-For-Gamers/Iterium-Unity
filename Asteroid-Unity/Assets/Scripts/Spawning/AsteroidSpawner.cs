using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{
    // Spawning of random asteroids at set intervals, random rotation and speed, targets an object for direction

    public class AsteroidSpawner : MonoBehaviour
    {
        [Header("Asteroid Spawning")]
        [SerializeField] private float spawnInterval = 5;
        [SerializeField] private float minSpeed = 3;
        [SerializeField] private float maxSpeed = 7;
        [SerializeField] private bool spawnOnceOnly;
        [SerializeField] private GameObject target; //asteroids move towards this scene target

        [SerializeField] private float speed;

        private void Start()
        {
            if (!spawnOnceOnly)
            {
                InvokeRepeating("SpawnAsteroid", 0, spawnInterval);
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
            speed = Random.Range(minSpeed, maxSpeed + 1);

            //Look and move towards target
            spawnedAsteroid.transform.LookAt(target.transform);
            spawnedAsteroid.GetComponent<Rigidbody>().velocity = spawnedAsteroid.transform.forward * speed;

            //Random rotation
            spawnedAsteroid.transform.rotation = Random.rotation;
            return spawnedAsteroid;
        }
    }
}