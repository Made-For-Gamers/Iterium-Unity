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

    [Header("NPC Bullet Pooling")]
    [SerializeField] private int capacityNpc = 10;
    [SerializeField] private int maxCapacityNpc = 15;
     
    public static ObjectPool<GameObject> bulletPoolPlayer;
    public static ObjectPool<GameObject> bulletPoolNpc;

    private void Awake()
    {     
        //NPC Pool
        bulletPoolNpc = new ObjectPool<GameObject>(PoolNew_NPC, PoolGet, PoolReturn, PoolDestroy, false, capacityNpc, maxCapacityNpc);

        //Player Pool
        bulletPoolPlayer = new ObjectPool<GameObject>(PoolNew_Player, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
    }

    private GameObject PoolNew_NPC() //NPC bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(GameManager.Instance.npc.Character.Ship.Bullet.Bullet[0]);
    }

    // Add a PoolNew_P?() method, for every player pool above to indication the bullet type to spawn.
    private GameObject PoolNew_Player() //Player 1 bullet type
    {
        //Instantiate a new asteroid
        return Instantiate(GameManager.Instance.player.Character.Ship.Bullet.Bullet[GameManager.Instance.player.BulletLvl]);
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
