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
    [SerializeField] private SO_Players players;
    public static ObjectPool<GameObject>[] bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<GameObject>[2];
        bulletPool[0] = new ObjectPool<GameObject>(PoolNew_P1, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        bulletPool[1] = new ObjectPool<GameObject>(PoolNew_P2, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
    }

    private GameObject PoolNew_P1()
    {
        //Instantiate a new asteroid
        return Instantiate(players.Players[0].Ship.Bullet.BulletLvl3);
    }
    private GameObject PoolNew_P2()
    {
        //Instantiate a new asteroid
        return Instantiate(players.Players[1].Ship.Bullet.BulletLvl3);
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
