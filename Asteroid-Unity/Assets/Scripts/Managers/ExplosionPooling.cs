using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{
    // Pooling of explosion effects
    public class ExplosionPooling : MonoBehaviour
    {
        [Header("Explosion Pooling")]
        [SerializeField] private int capacity = 10;
        [SerializeField] private int maxCapacity = 15;
        [SerializeField] SO_GameObjects explosion;

        //Pool
        public static ObjectPool<GameObject> explosionPool;

        void Awake()
        {
            explosionPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }

        //Instantiate a new explosion
        private GameObject PoolNew()
        {
            //Instantiate a new asteroid
            return Instantiate(explosion.GetRandomGameObject());
        }
        //Get an explosion from the pool
        private void PoolGet(GameObject obj)
        {
            obj.SetActive(true);
        }
        //Release an explosion
        private void PoolReturn(GameObject obj)
        {
            obj?.SetActive(false);
        }
        //Destroy explosion in the pool
        private void PoolDestroy(GameObject obj)
        {
            Destroy(obj);
        }
    }
}