using UnityEngine;

namespace Iterium
{
    /// <summary>
    /// NPC ship script that handles...
    /// * Firing (rate, target)
    /// * De-spawn when leaving the screen
    /// </summary>

    public class NPCController : MonoBehaviour
    {
        [Header("Bullet")]
        [SerializeField] private float fireDelay = 2f;
        [SerializeField] private float fireInterval = 0.5f;

        private int target;

        private void Start()
        {
            InvokeRepeating("Fire", fireDelay, fireInterval);
        }

        //Fire
        private void Fire()
        {
            GameObject bullet = BulletPooling.bulletPoolNpc.Get();
            bullet.transform.position = transform.position;
            if (GameManager.Instance.targetPlayer.gameObject && GameManager.Instance.targetAi.gameObject)
            {
                //Radomly attack player or AI
                target = Random.Range(1, 3);
                switch (target)
                {
                    case 1:
                        bullet.transform.LookAt(GameManager.Instance.targetPlayer.transform);
                        break;
                    case 2:
                        bullet.transform.LookAt(GameManager.Instance.targetAi.transform);
                        break;
                }
            }
            //Attack player
            else if (GameManager.Instance.targetPlayer.gameObject)
            {

                bullet.transform.LookAt(GameManager.Instance.targetPlayer.transform);
            }
            //Attack AI
            else if (GameManager.Instance.targetAi.gameObject)
            {

                bullet.transform.LookAt(GameManager.Instance.targetAi.transform);
            }

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.npcPlayer.Character.Ship.Bullet.Speed;

            //Increase NPC velocity if ship speed becomes too slow
            if (transform.GetComponent<Rigidbody>().velocity.x <= 2f)
            {
                transform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6, 7), 0, Random.Range(-6, 7));
            }
        }

        //Destroy NPC when leaving the screen
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}