using UnityEngine;
using UnityEngine.Pool;

public class BulletPooling : MonoBehaviour
{
    /// <summary>
    /// Pool spawned/de-spawn bullets to improve performance
    /// Used by players and NPCs
    /// </summary>
    [Header("Player Bullet Pooling")]
    [SerializeField] private int capacity = 25;
    [SerializeField] private int maxCapacity = 30; 
    [SerializeField] private SO_Players players; //Drag in the ScriptableObject list for players

    [Header("NPC Bullet Pooling")]
    [SerializeField] private int capacityNpc = 10;
    [SerializeField] private int maxCapacityNpc = 15;
    [SerializeField] private SO_Players npcs; //Drag in the ScriptableObject list for NPCs
  
    public static ObjectPool<GameObject>[] bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<GameObject>[players.Players.Count + npcs.Players.Count];

        //NPC Pool
        bulletPool[0] = new ObjectPool<GameObject>(PoolNew_NPC, PoolGet, PoolReturn, PoolDestroy, false, capacityNpc, maxCapacityNpc);

        //Player Pools - Add an additional pool below when increasing the player number.
        bulletPool[1] = new ObjectPool<GameObject>(PoolNew_P1, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        bulletPool[2] = new ObjectPool<GameObject>(PoolNew_P2, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        bulletPool[3] = new ObjectPool<GameObject>(PoolNew_P3, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
    }

    private GameObject PoolNew_NPC() //NPC bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(npcs.Players[0].Ship.Bullet.BulletLvl1);
    }

    // Add a PoolNew_P?() method, for every player pool above to indication the bullet type to spawn.
    private GameObject PoolNew_P1() //Player 1 bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(players.Players[0].Ship.Bullet.BulletLvl3);
    }
    private GameObject PoolNew_P2() //Player 2 bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(players.Players[1].Ship.Bullet.BulletLvl3);
    }

    private GameObject PoolNew_P3() //Player 3 bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(players.Players[2].Ship.Bullet.BulletLvl3);
    }

    //Get bullet from the pool
    private void PoolGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    //Return a bullet to the pool
    private void PoolReturn(GameObject obj)
    {
        obj?.SetActive(false);
    }

    //Destroy a bullet in the pool
    private void PoolDestroy(GameObject obj)
    {
        Destroy(obj);
    }
}
