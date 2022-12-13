using UnityEngine;
using UnityEngine.Pool;

public class BulletPooling : MonoBehaviour
{
    /// <summary>
    /// Pool spawned/de=spawned bullets to improve performance
    /// </summary>
    [Header("Bullet Pooling")]
    [SerializeField] private int capacity = 25;
    [SerializeField] private int maxCapacity = 30;
    [SerializeField] private SO_Player player;
    public static ObjectPool<GameObject> bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
    }

    private GameObject PoolNew()
    {
        //Instantiate a new asteroid
        return Instantiate(player.Ship.Bullet.BulletLvl1);
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
