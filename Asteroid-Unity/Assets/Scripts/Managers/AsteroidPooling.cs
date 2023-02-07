using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{
    public class AsteroidPooling : MonoBehaviour
    {
        /// <summary>
        /// Pool spawned/de=spawned asteroids to improve performance
        /// </summary>
        [Header("Asteroid Pooling")]
        [SerializeField] private int capacity = 30;
        [SerializeField] private int maxCapacity = 50;
        [SerializeField] private SO_GameObjects asteroids;
        public static ObjectPool<GameObject> asteroidPool;

        private void Awake()
        {
            asteroidPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }

        private GameObject PoolNew()
        {
            //Instantiate a new asteroid
            return Instantiate(asteroids.GetRandomGameObject());
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
    }
}