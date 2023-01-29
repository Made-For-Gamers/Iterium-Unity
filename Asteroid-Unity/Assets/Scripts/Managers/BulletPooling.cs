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

    [Header("AI Bullet Pooling")]
    [SerializeField] private int capacityAi = 10;
    [SerializeField] private int maxCapacityAi = 15;

    [Header("NPC Bullet Pooling")]
    [SerializeField] private int capacityNpc = 6;
    [SerializeField] private int maxCapacityNpc = 10;
     
    public static ObjectPool<GameObject> bulletPoolPlayer;
    public static ObjectPool<GameObject> bulletPoolAi;
    public static ObjectPool<GameObject> bulletPoolNpc;

    private void Awake()
    {     
         //Player Pool
        bulletPoolPlayer = new ObjectPool<GameObject>(PoolNew_Player, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);

        //Player Pool
        bulletPoolAi = new ObjectPool<GameObject>(PoolNew_Ai, PoolGet, PoolReturn, PoolDestroy, false, capacityAi, maxCapacityAi);

        //NPC Pool
        bulletPoolNpc = new ObjectPool<GameObject>(PoolNew_NPC, PoolGet, PoolReturn, PoolDestroy, false, capacityNpc, maxCapacityNpc);
    }

    // Add a PoolNew method for each firing character (bullet type)
    private GameObject PoolNew_Player() //Player 1 bullet
    {
        //Instantiate new player bullet
        return Instantiate(GameManager.Instance.player.Character.Ship.Bullet.Bullet[GameManager.Instance.bulletLvl-1]);
      
    }
    private GameObject PoolNew_Ai() //AI bullet
    {
        //Instantiate new AI bullet
        GameObject aiBullet = Instantiate(GameManager.Instance.aiPlayer.Character.Ship.Bullet.Bullet[0]);
        Destroy(aiBullet.GetComponent<Bullet>());
        aiBullet.AddComponent<BulletAI>();
        return aiBullet;
    }

    private GameObject PoolNew_NPC() //NPC bullet
    {
        //Instantiate new NPC bullet
        return Instantiate(GameManager.Instance.npcPlayer.Character.Ship.Bullet.Bullet[0]);
    }

    //Get a bullet from the pool
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
