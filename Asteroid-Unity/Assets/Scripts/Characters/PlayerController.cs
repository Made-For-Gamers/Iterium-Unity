using UnityEngine;
using System.Threading.Tasks;

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
        transform.Rotate(0, input.rotateInput.x * GameManager.Instance.player.Character.Ship.TurnSpeed * Time.deltaTime, 0);
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
        rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * GameManager.Instance.player.Character.Ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Shield
    private void Shield()
    {
        if (input.isShield & !isShielding) //Deploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            ShieldTime();
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
    private async void ShieldTime()
    {
        await Task.Delay(GameManager.Instance.player.Character.Ship.ShieldTime);
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

            DestroyShip();
        }
    }

    async void DestroyShip()
    {

        for (int i = 0; i < 3; i++)
        {
            //Spawn new explosion
            GameObject explosionObject = ExplosionPooling.explosionPool.Get();
            explosionObject.transform.position = transform.position;
            explosionObject.transform.rotation = transform.rotation;
            explosionObject.transform.localScale = new Vector3(i, i, i);
            await Task.Delay(150);
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
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = spawnPoint.position;
    }

}
