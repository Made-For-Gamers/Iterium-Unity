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
    [SerializeField] private float fireInterval = 0.6f;
    [SerializeField] private int descisionCycle = 3; //Bullets to fire at a target before deciding on changing target

    private Transform firePosition;
    private GameObject shield;
    private float shieldCooldown;
    private bool isShielding;
    private GameObject thrusters;
    private bool isThrusting;
    private Rigidbody rigidBody;
    private int shots;
    private bool attackNPC;

    private void Start()
    {
        shield = transform.GetChild(0).gameObject;
        thrusters = transform.GetChild(1).gameObject;
        firePosition = transform.GetChild(2);
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

    //AI Ship targeting, either the player or NPC
    private void Rotate()
    {
        if (GameManager.Instance.aiTarget.gameObject != null)
        {
            transform.LookAt(GameManager.Instance.aiTarget.transform);
        }

    }

    //Ship Firing
    private void Fire()
    {
        if (GameManager.Instance.aiTarget.gameObject != null)
        {
            GameObject bullet = BulletPooling.bulletPoolAi.Get();
            bullet.transform.position = firePosition.position;
            if (attackNPC == false && shots == 0)
            {
                if (GameManager.Instance.aiTarget.name == "NPC") //If there is an NPC in the scene randomly decide to target it or not
                {
                    int rnd = Random.Range(1, 3);
                    if (rnd == 1)
                    {
                        GameManager.Instance.FindAiTarget(true); //change target to NPC
                        attackNPC = true;
                    }
                }
            }
            bullet.transform.LookAt(GameManager.Instance.aiTarget.transform);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.aiPlayer.Character.Ship.Bullet.Speed;
            shots++;
            if (shots >= descisionCycle)
            {
                shots = 0;
                if (attackNPC) // re-target player at end of the decision cycle
                {
                    attackNPC = false;
                    GameManager.Instance.FindAiTarget(false);
                }
            }
        }
    }


    //Ship Thrust
    private void Thrust()
    {
        if (rigidBody.velocity.z >= 0f && GameManager.Instance.aiTarget.gameObject != null)
        {
            if (!isThrusting)
            {
                isThrusting = true;
                thrusters.SetActive(true);
            }
            rigidBody.AddRelativeForce(new Vector3(0, 0, 0.1f * (GameManager.Instance.aiPlayer.Character.Ship.Thrust * (GameManager.Instance.aiPlayer.SpeedLvl + 1)) * Time.deltaTime), ForceMode.Force);
        }
        else
        {
            isThrusting = false;
            thrusters.SetActive(false);
        }
    }

    //Ship Shield
    private void Shield()
    {
        if (!isShielding)
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = GameManager.Instance.aiPlayer.Character.Ship.ShieldCooldown;
        }

        if (shieldCooldown > 0) 
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

    //Calculate the damage when the ship is struck. Taking account of shield strength and striking bullets power.
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
            gameObject.GetComponent<BoxCollider>().enabled = false;
            if (GameManager.Instance.aiPermadeath)
            {
                GameManager.Instance.aiPlayer.Lives--;
            }
            if (GameManager.Instance.aiPlayer.Lives <= 0)
            {
                GameManager.Instance.GameOver();
            }
            StartCoroutine(DestroyShip());
        }
    }

    //Destroy this ship
    IEnumerator DestroyShip()
    {

        for (int i = 0; i < 3; i++)
        {
            //Spawn new explosion
            GameObject explosionObject = ExplosionPooling.explosionPool.Get();
            SoundManager.Instance.PlayShipExplosion();
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
        GameManager.Instance.SpawnAi(GameManager.Instance.deathRespawnTime);
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
