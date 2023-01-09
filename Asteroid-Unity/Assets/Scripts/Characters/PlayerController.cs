using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Player ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;   
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject shield;  

    [HideInInspector] public Transform spawnPoint;   

    private float shieldCooldown;
    private bool isShielding;
    private Rigidbody rigidBody;

    void Start()
    {
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
        transform.Rotate(0, input.rotateInput.x * Singleton.Instance.player.Ship.TurnSpeed * Time.deltaTime, 0);
    }

    //Firing
    private void Fire()
    {
        if (input.isfire)
        {
            GameObject bullet = BulletPooling.bulletPoolPlayer.Get();           
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * Singleton.Instance.player.Ship.Bullet.Speed;           
            input.isfire = false;
        }
    }

    //Ship Thrust
    private void Thrust()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * Singleton.Instance.player.Ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Shield
    private void Shield()
    {
        if (input.isShield & !isShielding) //Deploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = Singleton.Instance.player.Ship.ShieldCooldown;
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
        yield return new WaitForSeconds(Singleton.Instance.player.Ship.ShieldTime);
        shield.SetActive(false);

    }

    public void BulletHit(float firePower)
    {
        if (isShielding)
        {
            Singleton.Instance.player.Health -= (int)(firePower / Singleton.Instance.player.Ship.ShieldPower);
        }
        else
        {
            Singleton.Instance.player.Health -= (int)firePower;
        }
        if (Singleton.Instance.player.Health <= 0)
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

    //Remove ship when it leaves the screen
    private void OnBecameInvisible()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = spawnPoint.position;
    }
  
}
