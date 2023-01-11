using UnityEngine;
using System.Collections;
using UnityEngine.Windows;

/// <summary>
/// AI ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// </summary>
public class AIController : MonoBehaviour
{
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
    }

    private void Update()
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
        if (player.gameObject != null)
        {
            transform.LookAt(player.transform);
        }
    }

    //Firing
    private void Fire()
    {
        //if (input.isfire)
        //{
        //    GameObject bullet = BulletPooling.bulletPoolPlayer.Get();
        //    bullet.transform.position = firePosition.position;
        //    bullet.transform.rotation = firePosition.rotation;
        //    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.player.Character.Ship.Bullet.Speed;
        //    input.isfire = false;
        //}
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
            shieldCooldown = GameManager.Instance.player.Character.Ship.ShieldCooldown;
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
        yield return new WaitForSeconds(GameManager.Instance.player.Character.Ship.ShieldTime);
        shield.SetActive(false);

    }

    public void BulletHit(float firePower)
    {
        if (isShielding)
        {
            GameManager.Instance.player.Health -= (int)(firePower / GameManager.Instance.player.Character.Ship.ShieldPower);
        }
        else
        {
            GameManager.Instance.player.Health -= (int)firePower;
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
            ExplosionPooling.explosionPool.Release(explosionObject);
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
