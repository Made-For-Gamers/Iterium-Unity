using UnityEngine;
using System.Collections;

/// <summary>
/// Player ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// * Health decrease calculation (ship hit)
/// * Ship destroy
/// * Screen warping
/// </summary>

[RequireComponent(typeof(InputManager))]

public class PlayerController : MonoBehaviour
{
    private InputManager input;
    private Transform firePosition;
    private GameObject shield;
    private float shieldCooldown;
    private bool isShielding;
    private GameObject thrusters;
    private bool isThrusting;
    private Rigidbody rigidBody;

    void Start()
    {
        input = GetComponent<InputManager>();
        shield = transform.GetChild(0).gameObject;
        thrusters = transform.GetChild(1).gameObject;
        firePosition = transform.GetChild(2);
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Rotate();
        Fire();
        Shield();
        Warp();
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    //Ship rotation
    private void Rotate()
    {
        transform.Rotate(0, input.rotateInput.x * (GameManager.Instance.player.Character.Ship.TurnSpeed * GameManager.Instance.speedLvl) * Time.deltaTime, 0);
    }

    //Firing
    private void Fire()
    {
        if (input.isfire)
        {
            GameObject bullet = BulletPooling.bulletPoolPlayer.Get();
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.player.Character.Ship.Bullet.Speed;
            input.isfire = false;
        }
    }

    //Ship Thrust
    private void Thrust()
    {
        if (input.thrustInput.y > 0.1f)
        {
            if (!isThrusting)
            {
                //StartCoroutine(Thrusters());
                isThrusting = true;
                thrusters.SetActive(true);
            }
            rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * (GameManager.Instance.player.Character.Ship.Thrust * GameManager.Instance.speedLvl) * Time.deltaTime), ForceMode.Force);
        }
        else
        {
            isThrusting = false;
            thrusters.SetActive(false);
        }
    }

    //Shield
    private void Shield()
    {
        if (input.isShield & !isShielding) //Deploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = GameManager.Instance.player.Character.Ship.ShieldCooldown;
        }

        if (shieldCooldown > 0) //Cooldown countdown
        {
            shieldCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            input.isShield = false;
            isShielding = false;
        }
    }

    private void Warp()
    {
        if (input.isWarping)
        { 
            transform.position = GameManager.Instance.playerSpawner.position;
            //rigidBody.velocity = Vector3.zero;
            input.isWarping = false;
        }
    }

    //Shield cooldown timer
    private IEnumerator ShieldTime()
    {
        yield return new WaitForSeconds(GameManager.Instance.player.Character.Ship.ShieldTime);
        shield.SetActive(false);
    }

    public void BulletHit(float firePower)
    {
        if (isShielding)
        {
            GameManager.Instance.player.Health -= (int)(firePower / (GameManager.Instance.player.Character.Ship.ShieldPower * GameManager.Instance.shieldLvl));
        }
        else
        {
            GameManager.Instance.player.Health -= (int)firePower;
        }
        if (GameManager.Instance.player.Health <= 0)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.player.Lives--;
            if (GameManager.Instance.player.Lives <= 0)
            {
                GameManager.Instance.GameOver();
            }
            StartCoroutine(DestroyShip());
        }
    }

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
        GameManager.Instance.SpawnPlayer(GameManager.Instance.deathRespawnTime);
        Destroy(this.gameObject);
    }

    //Remove ship when it leaves the screen and re-spawn
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
