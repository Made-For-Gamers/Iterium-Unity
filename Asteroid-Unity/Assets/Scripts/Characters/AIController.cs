using UnityEngine;
using System.Collections;

/// <summary>
/// AI player ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// * Health decrease calculation (ship hit)
/// * Ship destroy
/// * Screen warping
/// </summary>
public class AIController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float fireStart = 2f;
    [SerializeField] private float fireInterval = 0.9f;
    [SerializeField] private int descisionCycle = 3; //number of bullets to fire at a target before deciding to changing targets

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

    //Ship rotation targeting the player or NPC
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
            if (attackNPC == false && shots == 0)
            {
                if (GameObject.Find("NPC")) //If there is an NPC in the scene randomly decide to target it or not
                {
                    int rnd = Random.Range(1, 3);
                    if (rnd == 1)
                    {
                        fireTarget = GameObject.Find("NPC"); //change target to NPC
                        attackNPC = true;
                    }
                }
            }
            bullet.transform.LookAt(fireTarget.transform);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.aiPlayer.Character.Ship.Bullet.Speed;
            shots++;
            if (shots >= descisionCycle)
            {
                shots = 0;
                if (attackNPC) // re-target player when the decision cycle is reached
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
            rigidBody.AddRelativeForce(new Vector3(0, 0, 0.1f * (GameManager.Instance.aiPlayer.Character.Ship.Thrust * GameManager.Instance.aiPlayer.SpeedLvl) * Time.deltaTime), ForceMode.Force);
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

        if (shieldCooldown > 0) //Shield cooldown 
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
            GameManager.Instance.aiPlayer.Health -= (int)(firePower / (GameManager.Instance.aiPlayer.Character.Ship.ShieldPower * GameManager.Instance.aiPlayer.ShieldLvl));
        }
        else
        {
            GameManager.Instance.aiPlayer.Health -= (int)firePower;
        }
        if (GameManager.Instance.aiPlayer.Health <= 0)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            GameManager.Instance.aiPlayer.Lives--;
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
