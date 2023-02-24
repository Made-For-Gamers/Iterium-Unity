using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    /// <summary>
    /// NPC ship script that handles...
    /// * Firing (rate, target)
    /// * De-spawn when leaving the screen
    /// </summary>

    public class NPCController : MonoBehaviour, IDamage
    {
        [Header("Bullet")]
        [SerializeField] private float fireDelay = 2f;
        [SerializeField] private float fireInterval = 0.4f;

        public static event Action<string> NpcDamage;
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
            bullet.GetComponent<BulletNpc>().firePower = GameManager.Instance.npcPlayer.Faction.Ship.Bullet.FirePower;
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

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.npcPlayer.Faction.Ship.Bullet.Speed;

            //Increase NPC velocity if ship speed becomes too slow
            if (transform.GetComponent<Rigidbody>().velocity.x <= 2f)
            {
                transform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6, 7), 0, Random.Range(-6, 7));
            }
        }

        public void Damage(float firePower, string attacker)
        {
            NpcDamage.Invoke(attacker);
            DestroyShip();
        }

        //Destroy NPC when leaving the screen
        private void OnBecameInvisible()
        {          
            DestroyShip();
        }

        private void DestroyShip()
        {
            Destroy(gameObject);
        }
    }
}