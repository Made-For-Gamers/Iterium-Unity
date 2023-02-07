using UnityEngine;
using UnityEngine.Pool;

namespace Iterium
{

    /// <summary>
    /// Pool spawned/de=spawned explostion effects to improve performance
    /// </summary>
    public class ExplosionPooling : MonoBehaviour
    {
        [Header("Explosion Pooling")]
        [SerializeField] private int capacity = 10;
        [SerializeField] private int maxCapacity = 15;
        [SerializeField] SO_GameObjects explosion;
        public static ObjectPool<GameObject> explosionPool;

        void Awake()
        {
            explosionPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }

        private GameObject PoolNew()
        {
            //Instantiate a new asteroid
            return Instantiate(explosion.GetRandomGameObject());
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