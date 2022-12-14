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
    [SerializeField] private InputManager input;
    [SerializeField] private SO_Players playerList;
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform player1Spawnpoint;
    [SerializeField] private Transform player2Spawnpoint;
    private float shieldCooldown;
    private Rigidbody rigidBody;
    public static bool isShielding;
    public SO_Player player;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        switch (transform.tag)
        {
            case "Player1":
                player = playerList.Players[0];
                break;
            case "Player2":
                player = playerList.Players[1];
                break;
        }
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
            GameObject bullet = BulletPooling.bulletPool.Get();
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * player.Ship.Bullet.Speed;
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
        if (input.isShield & !isShielding) //Desploy Shield
        {
            shield.SetActive(true);
            isShielding = true;
            StartCoroutine(ShieldTime());
            shieldCooldown = player.Ship.ShieldCooldown;
        }
        else if (shieldCooldown > 0) //Cooldown countdown
        {
            shieldCooldown -= 1 * Time.deltaTime;
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

    //Move ship when it leaves the screen
    private void OnBecameInvisible()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (transform.tag == "Player2")
        {
            transform.position = player2Spawnpoint.position;
        }
        else
        {
            transform.position = player1Spawnpoint.position;
        }
    }
}
