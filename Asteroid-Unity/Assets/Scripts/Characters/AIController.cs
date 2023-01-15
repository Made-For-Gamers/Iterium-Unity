using UnityEngine;
using System.Collections;

/// <summary>
/// AI ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// </summary>
public class AIController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float fireStart = 1f;
    [SerializeField] private float fireInterval = 0.5f;

    [HideInInspector] public Transform spawnPoint;

    private GameObject player;
    private Transform firePosition;
    private GameObject shield;
    private float shieldCooldown;
    private bool isShielding;
    private Rigidbody rigidBody;

    private void Start()
    {
        player = GameObject.Find("Player");
        shield = transform.GetChild(0).gameObject;
        firePosition = transform.GetChild(1);
        rigidBody = GetComponent<Rigidbody>();
        InvokeRepeating("Fire", fireStart, fireInterval);
    }

    private void Update()
    {
        Rotate();
        Shield();
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    //Ship rotation
    private void Rotate()
    {
        if (player.gameObject != null)
        {
            transform.LookAt(player.transform);
        }
    }

    //Firing
    private void Fire()
    {
        if (player.gameObject != null)
        {
            GameObject bullet = BulletPooling.bulletPoolAi.Get();
            bullet.transform.position = firePosition.position;
            bullet.transform.LookAt(player.transform);
            Destroy(bullet.GetComponent<Bullet>());
            bullet.AddComponent<BulletAI>();
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.aiPlayer.Character.Ship.Bullet.Speed;
        }
    }

    //Ship Thrust
    private void Thrust()
    {
        //  rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * GameManager.Instance.player.Character.Ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Shield
    private void Shield()
    {
        if (!isShielding) //Deploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = GameManager.Instance.aiPlayer.Character.Ship.ShieldCooldown;
        }

        if (shieldCooldown > 0) //Cooldown countdown
        {
            shieldCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            isShielding = false;
        }
    }

    //Shield cooldown timer
    private IEnumerator ShieldTime()
    {
        yield return new WaitForSeconds(GameManager.Instance.aiPlayer.Character.Ship.ShieldTime);
        shield.SetActive(false);

    }

    public void BulletHit(float firePower)
    {
        if (isShielding)
        {
            GameManager.Instance.aiPlayer.Health -= (int)(firePower / GameManager.Instance.aiPlayer.Character.Ship.ShieldPower);
        }
        else
        {
            GameManager.Instance.aiPlayer.Health -= (int)firePower;
        }
        if (GameManager.Instance.aiPlayer.Health <= 0)
        {

            StartCoroutine(DestroyShip());
        }
    }

    IEnumerator DestroyShip()
    {

        for (int i = 0; i < 3; i++)
        {
            //Spawn new explosion
            GameObject explosionObject = ExplosionPooling.explosionPool.Get();
            explosionObject.transform.position = transform.position;
            explosionObject.transform.rotation = transform.rotation;
            explosionObject.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.15f);
            //Prepare to return explosion to pool
            explosionObject.transform.localScale = new Vector3(1, 1, 1);
            if (gameObject.activeSelf)
            {
                ExplosionPooling.explosionPool.Release(explosionObject);
            }
        }
        Destroy(this.gameObject);
    }


    //Remove AI when it leaves the screen and re-spawn
    private void OnBecameInvisible()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = spawnPoint.position;
    }
}
