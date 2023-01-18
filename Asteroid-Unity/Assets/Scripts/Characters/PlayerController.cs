using UnityEngine;
using System.Collections;

/// <summary>
/// Player ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// </summary>
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Transform spawnPoint;

    private InputManager input;
    private Transform firePosition;
    private GameObject shield;  
    private float shieldCooldown;
    private bool isShielding;
    private Rigidbody rigidBody;

    void Start()
    {
        input = GetComponent<InputManager>();
        shield = transform.GetChild(0).gameObject;
        firePosition = transform.GetChild(1);
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Rotate();
        Fire();
        Shield();
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    //Ship rotation
    private void Rotate()
    {
        transform.Rotate(0, input.rotateInput.x * (GameManager.Instance.player.Character.Ship.TurnSpeed * GameManager.Instance.player.SpeedLvl) * Time.deltaTime, 0);
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
        rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * (GameManager.Instance.player.Character.Ship.Thrust * GameManager.Instance.player.SpeedLvl) * Time.deltaTime), ForceMode.Force);
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
            GameManager.Instance.player.Health -= (int)(firePower / (GameManager.Instance.player.Character.Ship.ShieldPower * GameManager.Instance.player.ShieldLvl));
           // print((int)(firePower / (GameManager.Instance.player.Character.Ship.ShieldPower * GameManager.Instance.player.ShieldLvl)));
        }
        else
        {
            GameManager.Instance.player.Health -= (int)firePower;
           // print((int)(firePower));
        }
        if (GameManager.Instance.player.Health <= 0)
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
