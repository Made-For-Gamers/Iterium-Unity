using UnityEngine;
using System.Collections;

/// <summary>
/// Player ship script that handles...
/// Movement
/// Firing
/// Shield
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private SO_Ship ship;
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject shield;
    private Vector3 shipRotate;
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

    //Rorate the ship
    private void Rotate()
    {
        transform.Rotate(0, input.rotateInput.x * ship.TurnSpeed * Time.deltaTime, 0);
    }

    //Fire a bullet
    private void Fire()
    {
        if (input.isfire)
        {
            GameObject bullet = BulletPooling.bulletPool.Get();
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * ship.Bullet.Speed;
            input.isfire = false;
        }
    }

    //Move the ship forward
    private void Thrust()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, input.thrustInput.y * ship.Thrust * Time.deltaTime), ForceMode.Force);
    }

    //Shield
    private void Shield()
    {
        if (input.isShield)
        {
            shield.SetActive(true);
            StartCoroutine(ShieldTime());
            input.isShield = false;
        }
    }

    private IEnumerator ShieldTime()
    {
        yield return new WaitForSeconds(ship.ShieldTime);
        shield.SetActive(false);
    }

    //Move ship when it leaves the screen
    private void OnBecameInvisible()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
