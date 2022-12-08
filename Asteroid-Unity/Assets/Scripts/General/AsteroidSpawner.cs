using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawning of random asteroids at set intervals, random rotation and speed, targets an object for direction
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private SO_GameObjects asteroids;
    [SerializeField] private float spawnTime;
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private GameObject target;
    private float speed;

    private void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0, spawnTime);
    }

    private void SpawnAsteroid()
    {
        //Instantiate a new asteroid    
        GameObject spawnedAsteroid = Instantiate(asteroids.GetRandomGameObject());
        spawnedAsteroid.transform.position = transform.position;
        speed = Random.Range(speedMin, speedMax + 1);

        //Look and move towards target
        spawnedAsteroid.transform.LookAt(target.transform); 
        spawnedAsteroid.GetComponent<Rigidbody>().velocity = spawnedAsteroid.transform.forward * speed;

        //Random rotation after Look and direction is set
        spawnedAsteroid.transform.rotation = Random.rotation; 
    }
}
