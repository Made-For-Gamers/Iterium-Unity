using UnityEngine;
using UnityEngine.Pool;


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
    public static bool isAsteroidPooling = true;
    [Header("Asteroid Pooling")]
    [SerializeField] private int capacity = 30;
    [SerializeField] private int maxCapacity = 50;
    [HideInInspector] public static ObjectPool<GameObject> asteroidPool;
    private float speed;
    private GameObject spawnedAsteroid;

    private void Start()
    {
        if (isAsteroidPooling)
        {
            asteroidPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }
        InvokeRepeating("SpawnAsteroid", 0, spawnTime);
    }

    private GameObject PoolNew()
    {
        //Instantiate a new asteroid
        GameObject newAsteroid = Instantiate(asteroids.GetRandomGameObject());
        return newAsteroid;
    }

    private void PoolGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void PoolReturn(GameObject obj)
    {
        obj?.SetActive(false);
    }

    private void PoolDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private GameObject SpawnAsteroid()
    {     
        //Instantiate an asteroid
        spawnedAsteroid = isAsteroidPooling ? asteroidPool.Get() : Instantiate(asteroids.GetRandomGameObject());
      
        spawnedAsteroid.transform.position = transform.position;
        speed = Random.Range(speedMin, speedMax + 1);

        //Look and move towards target
        spawnedAsteroid.transform.LookAt(target.transform);
        spawnedAsteroid.GetComponent<Rigidbody>().velocity = spawnedAsteroid.transform.forward * speed;

        //Random rotation after Look and direction is set
        spawnedAsteroid.transform.rotation = Random.rotation;
        return spawnedAsteroid;
    }


}
