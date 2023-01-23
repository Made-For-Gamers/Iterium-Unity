using UnityEngine;

/// <summary>
/// NPC ship script that handles...
/// * Firing (rate, target)
/// * De-spawn when leaving the screen
/// </summary>

public class NPCController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float fireStart = 3f;
    [SerializeField] private float fireInterval = 0.8f;

    private int target;

    private void Start()
    {
        InvokeRepeating("Fire", fireStart, fireInterval);
    }

    private void Fire()
    {
        GameObject bullet = BulletPooling.bulletPoolNpc.Get();
        bullet.transform.position = transform.position;
        if (GameManager.Instance.player != null && GameManager.Instance.aiPlayer != null)
        {
            //Radomly attack player or AI
            target = Random.Range(1, 3);
            switch (target)
            {
                case 1:
                    bullet.transform.LookAt(GameObject.Find("Player").transform);
                    break;
                case 2:
                    bullet.transform.LookAt(GameObject.Find("AI").transform);
                    break;
            }
        }
        else if (GameManager.Instance.player != null)
        {
            //Attack player
            bullet.transform.LookAt(GameObject.Find("Player").transform);
        }
        else
        {
            //Attack AI
            bullet.transform.LookAt(GameObject.Find("AI").transform);
        }

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.npcPlayer.Character.Ship.Bullet.Speed;

        //Increase NPC velocity if ship speed becomes too slow due to collision with asteroids
        if (transform.GetComponent<Rigidbody>().velocity.x <= 2f)
        {
            transform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6, 7), 0, Random.Range(-6, 7));
        }
    }

    //Remove NPC when leaving the screen
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
