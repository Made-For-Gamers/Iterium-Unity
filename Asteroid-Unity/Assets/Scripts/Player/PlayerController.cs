using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using System.ComponentModel;
using UnityEditor.ShaderGraph.Drawing;
using Unity.Collections;

/// <summary>
/// Player ship script that handles...
/// * Movement
/// * Firing
/// * Shield
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private SO_Players playerList;
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject shield;     
   
    [HideInInspector] public SO_Player player;
    [HideInInspector] public Transform spawnPoint; 
    [HideInInspector] public int playerNumber;
    
    private float shieldCooldown;
    private bool isShielding;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        player = playerList.Players[playerNumber - 1];
        player.Health = 100;
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
        transform.Rotate(0, input.rotateInput.x * player.Ship.TurnSpeed * Time.deltaTime, 0);
    }

    //Firing
    private void Fire()
    {
        if (input.isfire)
        {
            GameObject bullet = BulletPooling.bulletPool[playerNumber - 1].Get();
            bullet.GetComponent<Bullet>().player = player;
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * player.Ship.Bullet.Speed;
            bullet.GetComponentInParent<Bullet>().PlayerNumber = playerNumber;
            input.isfire = false;
        }
    }

    //Ship Thrust
    private void Thrust()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * player.Ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Shield
    private void Shield()
    {
        if (input.isShield & !isShielding) //Deploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = player.Ship.ShieldCooldown;
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
        yield return new WaitForSeconds(player.Ship.ShieldTime);
        shield.SetActive(false);

    }

    public void BulletHit(float firePower, SO_Player shootingPlayer)
    {
        if (isShielding)
        {
            player.Health -= (int)(firePower / player.Ship.ShieldPower);
        }
        else
        {
            player.Health -= (int)firePower;
        }
        if (player.Health <= 0)
        {
            shootingPlayer.Score += 1000;
            shootingPlayer.Xp += 250;
            StartCoroutine(DestroyShip());
        }
    }

    IEnumerator DestroyShip()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject explosionObject = ExplosionPooling.explosionPool.Get();
            explosionObject.transform.position = transform.position;
            explosionObject.transform.rotation = transform.rotation;
            explosionObject.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);
    }

    //Move ship when it leaves the screen
    private void OnBecameInvisible()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = spawnPoint.position;
    }
}
