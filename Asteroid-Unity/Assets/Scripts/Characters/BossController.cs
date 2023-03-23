using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    public class BossController : MonoBehaviour, IDamage
    {
        [Header("Boss Settings")]
        [SerializeField] private float minSpeed = 0.6f;
        [SerializeField] private float maxSpeed = 1f;
        [SerializeField] private float fireDelay = 2f;
        [SerializeField] private float fireInterval = 0.3f;

        private float speed;
        private int target;

        //Events
        public static event Action<string> BossDamage;
        public static event Action BossDestroy;

        private void Start()
        {
            speed = Random.Range(minSpeed, maxSpeed);
            InvokeRepeating("Fire", fireDelay, fireInterval);
        }

        void Update()
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }

        private void Fire()
        {
            //Xoid boss uses the same bullet as Xoid NPC
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
        }

        public void Damage(float firePower, string attacker)
        {
            BossDamage?.Invoke(attacker);
        }

        private void OnBecameInvisible()
        {
            BossDestroy?.Invoke();
            DestroyShip();
        }

        private void DestroyShip()
        {
            Destroy(gameObject);
        }
    }
}
