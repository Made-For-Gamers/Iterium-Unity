using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Player ship script for moving/firing/shield
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private SO_Ship ship;
    [SerializeField] private Transform firePosition;
    private Vector3 shipRotate;
    private Rigidbody rigidBody;
    public static bool isBulletPooling = true;
    [Header("Bullet Pooling")]
    [SerializeField] private int capacity = 25;
    [SerializeField] private int maxCapacity = 30;
    [HideInInspector] public static ObjectPool<GameObject> bulletPool;

    void Start()
    {
        if (isBulletPooling)
        {
            bulletPool = new ObjectPool<GameObject>(PoolNew, PoolGet, PoolReturn, PoolDestroy, false, capacity, maxCapacity);
        }
        rigidBody = GetComponent<Rigidbody>();
    }
    private GameObject PoolNew()
    {
        //Instantiate a new asteroid
        GameObject newBullet = Instantiate(ship.Bullet.BulletLvl1);     
        return newBullet;
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

    void Update()
    {
        Rotate();
        Fire();
    }
  
    private void FixedUpdate()
    {
        Thrust();
    }

    //Rorate the ship
    private void Rotate()
    {
        transform.Rotate(0 ,input.rotateInput.x * ship.TurnSpeed * Time.deltaTime ,0 );
    }

    //Fire a bullet
    private void Fire()
    {
        if (input.isfire)
        {
            GameObject bullet = isBulletPooling ? bulletPool.Get() : Instantiate(ship.Bullet.BulletLvl1);
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * ship.Bullet.Speed;
            input.isfire = false;
        }
    }

    //Move the ship forward
    private void Thrust()
    {
        rigidBody.AddRelativeForce(new Vector3 (0, 0, input.thrustInput.y * ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Move ship when it leaves the screen
    private void OnBecameInvisible()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
