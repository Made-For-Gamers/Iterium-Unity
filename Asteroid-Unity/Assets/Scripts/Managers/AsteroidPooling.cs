using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{
    // Pooling of asteroid spawning

    public class AsteroidPooling : MonoBehaviour
    {

        [Header("Asteroid Pooling")]
        [SerializeField] private int capacity = 30;
        [SerializeField] private int maxCapacity = 50;
        [SerializeField] private SO_GameObjects asteroids;

        //Pool
        public static ObjectPool<GameObject> asteroidPool;

        private void Awake()
        {
            asteroidPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }

        //Instantiate a new asteroid
        private GameObject PoolNew()
        {

            return Instantiate(asteroids.GetRandomGameObject());
        }

        //Get an asteroid from the pool
        private void PoolGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        //Release an asteroid
        private void PoolReturn(GameObject obj)
        {
            obj?.SetActive(false);
        }

        //Destroy an asteroid in the pool
        private void PoolDestroy(GameObject obj)
        {
            Destroy(obj);
        }
    }
}