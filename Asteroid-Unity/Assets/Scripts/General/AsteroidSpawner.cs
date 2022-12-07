using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] SO_Asteroids asteroids;
    [SerializeField] float spawnTime;
    [SerializeField] float speedMin;
    [SerializeField] float speedMax;
    [SerializeField] GameObject target;
    float speed;

    private void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0, spawnTime);
    }

    private void SpawnAsteroid()
    {
        int randomAsteroid = Random.Range(0, asteroids.Asteroids.Count);
        GameObject spawnedAsteroid = Instantiate(asteroids.Asteroids[randomAsteroid], transform.position, transform.rotation);
        spawnedAsteroid.transform.LookAt(target.transform);
        speed = Random.Range(speedMin, speedMax + 1);
        spawnedAsteroid.GetComponent<Rigidbody>().velocity = spawnedAsteroid.transform.forward * speed;
    }
}
