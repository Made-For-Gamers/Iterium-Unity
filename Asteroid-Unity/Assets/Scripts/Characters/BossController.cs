using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    public class BossController : MonoBehaviour
    {
        [Header("Boss Settings")]
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float fireDelay = 2f;
        [SerializeField] private float fireInterval = 0.5f;

        private float speed;
        private int target;
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

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * GameManager.Instance.npcPlayer.Faction.Ship.Bullet.Speed;
        }

        private void OnBecameInvisible()
        {
            BossDestroy.Invoke();
            DestroyShip();
        }

        private void DestroyShip()
        {
            Destroy(gameObject);
        }
    }
}
