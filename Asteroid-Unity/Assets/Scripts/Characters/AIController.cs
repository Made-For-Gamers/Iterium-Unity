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
    [SerializeField] private float fireStart = 2f;
    [SerializeField] private float fireInterval = 0.8f;

    [HideInInspector] public Transform spawnPoint;

    private Transform firePosition;
    private GameObject shield;
    private float shieldCooldown;
    private bool isShielding;
    private Rigidbody rigidBody;
    private int shots;
    private bool attackNPC;
    private GameObject fireTarget;

    private void Start()
    {
        fireTarget = GameObject.Find("Player");
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
        if (fireTarget.gameObject != null)
        {
            transform.LookAt(fireTarget.transform);
        }
    }

    //Firing
    private void Fire()
    {
        if (fireTarget.gameObject != null)
        {
            GameObject bullet = BulletPooling.bulletPoolAi.Get();
            bullet.transform.position = firePosition.position;
            Destroy(bullet.GetComponent<Bullet>());
            bullet.AddComponent<BulletAI>();
            if (attackNPC == false && shots == 0)
            {
                if (GameObject.Find("NPC"))
                {
                    int rnd = Random.Range(1, 3);
                    //Radomly attack player or AI
                    if (rnd == 1)
                    {
                        fireTarget = GameObject.Find("NPC");
                        attackNPC = true;
                    }
                }
            }
            bullet.transform.LookAt(fireTarget.transform);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.aiPlayer.Character.Ship.Bullet.Speed;
            shots++;
            if (shots >= 4)
            {
                shots = 0;
                if (attackNPC)
                {
                    attackNPC = false;
                    fireTarget = GameObject.Find("Player");
                }
            }
        }
    }


    //Ship Thrust
    private void Thrust()
    {
        if (rigidBody.velocity.x <= 1)
        {
            rigidBody.AddRelativeForce(new Vector3(0, 0, 0.1f * GameManager.Instance.player.Character.Ship.Thrust * Time.deltaTime), ForceMode.Force);
        }
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
        //Wrap ship to opposite side of the screen when exiting
        Vector3 viewPort = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 movePos = transform.position;
        if (viewPort.x > 1 || viewPort.x < 0)
        {
            movePos.x = -movePos.x;
        }
        if (viewPort.y > 1 || viewPort.y < 0)
        {
            movePos.z = -movePos.z;
        }
        transform.position = movePos;
    }
}
