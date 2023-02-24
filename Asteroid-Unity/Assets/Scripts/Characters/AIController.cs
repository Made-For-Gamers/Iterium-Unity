using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

namespace Iterium
{
    /// <summary>
    /// AI player ship script that handles...
    /// * Movement
    /// * Firing
    /// * Shield
    /// * Hit health calculation 
    /// * Destroy
    /// * Screen warping
    /// </summary>

    public class AIController : MonoBehaviour, IDamage
    {
        [Header("Bullet Settings")]
        [SerializeField] private float fireDelay = 2f;
        [SerializeField] private float fireInterval = 0.6f;
        [SerializeField] private float warpInterval = 15f;
        [SerializeField] private int decisionCycle = 3; //Number of bullets to fire in a targeting decision round

        private Transform firePosition;
        private GameObject shield;
        private bool isShielding;
        private GameObject thrusters;
        private bool isThrusting;
        private Rigidbody rigidBody;
        private int shots;
        private int target; // 0 = Dont fire / 1 = Player / 2 = NPC / 3 = Boss
        private int rnd;

        public static event Action AiDamage;

        private void Start()
        {
            shield = transform.GetChild(0).gameObject;
            thrusters = transform.GetChild(1).gameObject;
            firePosition = transform.GetChild(2);
            rigidBody = GetComponent<Rigidbody>();
            InvokeRepeating("Fire", fireDelay, fireInterval);
            InvokeRepeating("Shield", 0, GameManager.Instance.aiPlayer.Faction.Ship.ShieldCooldown);
            InvokeRepeating("Warp", warpInterval, warpInterval);
        }

        private void Update()
        {
            Rotate();
        }

        private void FixedUpdate()
        {
            Thrust();
        }

        //Rotate towards target
        private void Rotate()
        {
            if (target == 1 && GameManager.Instance.targetPlayer.gameObject)
            {
                transform.LookAt(GameManager.Instance.targetPlayer.transform);
            }
            else if (target == 2 && GameManager.Instance.targetNpc.gameObject)
            {
                transform.LookAt(GameManager.Instance.targetNpc.transform);
            }
            else if (target == 3 && GameManager.Instance.targetBoss.gameObject)
            {
                transform.LookAt(GameManager.Instance.targetBoss.transform);
            }
        }

        //Firing
        private void Fire()
        {
            //Check if current target is destoryed then reset descision cycle
            if (target == 1 && !GameManager.Instance.targetPlayer.gameObject)
            {
                shots = 0;
                target = 0;
            }
            else if (target == 2 && !GameManager.Instance.targetNpc.gameObject)
            {
                shots = 0;
                target = 0;
            }
            else if (target == 3 && !GameManager.Instance.targetBoss.gameObject)
            {
                shots = 0;
                target = 0;
            }

            if (target > 0)
            {
                GameObject bullet = BulletPooling.bulletPoolAi.Get();
                bullet.transform.position = firePosition.position;

                //Point bullet towards target
                if (target == 1 && GameManager.Instance.targetPlayer.gameObject)
                {
                    bullet.transform.LookAt(GameManager.Instance.targetPlayer.transform);
                }
                else if (target == 2 && GameManager.Instance.targetNpc.gameObject)
                {
                    bullet.transform.LookAt(GameManager.Instance.targetNpc.transform);
                }
                else if (target == 3 && GameManager.Instance.targetBoss.gameObject)
                {
                    bullet.transform.LookAt(GameManager.Instance.targetBoss.transform);
                }

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * (GameManager.Instance.aiPlayer.Faction.Ship.Bullet.Speed + ((GameManager.Instance.aiPlayer.Faction.Ship.Bullet.Speed / 5) * GameManager.Instance.aiPlayer.BulletLvl));
                bullet.GetComponent<BulletAI>().firePower = GameManager.Instance.aiPlayer.Faction.Ship.Bullet.FirePower;
                shots++;

                //reset decision round
                if (shots >= decisionCycle)
                {
                    shots = 0;
                }
            }

            //New target decision round
            if (shots == 0)
            {
                //Zero target if ther is no spawned Player/NPC/Boss
                if (!GameManager.Instance.targetNpc.gameObject && !GameManager.Instance.targetBoss.gameObject && !GameManager.Instance.targetPlayer.gameObject)
                {
                    target = 0;
                }
                //Target Player if there is no NPC/Boss
                else if (!GameManager.Instance.targetNpc.gameObject && !GameManager.Instance.targetBoss.gameObject)
                {
                    target = 1;
                }
                //Randomly select a target if there is 2 or more characters on screen
                else
                {
                    do
                    {
                        rnd = Random.Range(1, 4);
                        switch (rnd)
                        {
                            case 1: //Player tageted
                                if (GameManager.Instance.targetPlayer.gameObject)
                                {
                                    target = 1;
                                }
                                break;
                            case 2: //NPC targeted
                                if (GameManager.Instance.targetNpc.gameObject)
                                {
                                    target = 2;
                                }
                                break;
                            case 3: //Boss targeted
                                if (GameManager.Instance.targetBoss.gameObject)
                                {
                                    target = 3;
                                }
                                break;
                        }
                    }
                    while (target == 0);
                }
            }
        }

        //Thrust
        private void Thrust()
        {
            if (rigidBody.velocity.z > -1f)
            {
                if (!isThrusting)
                {
                    isThrusting = true;
                    thrusters.SetActive(true);
                }
                rigidBody.AddRelativeForce(new Vector3(0, 0, 0.2f * (GameManager.Instance.aiPlayer.Faction.Ship.Thrust + (GameManager.Instance.aiPlayer.Faction.Ship.Thrust / 5) * GameManager.Instance.aiPlayer.SpeedLvl) * Time.deltaTime), ForceMode.Force);
            }
            else if (isThrusting)
            {
                isThrusting = false;
                thrusters.SetActive(false);
            }
        }

        //Shield
        private void Shield()
        {
            if (shield.activeSelf)
            {
                shield.SetActive(false);
            }
            else
            {
                shield.SetActive(true);
            }
        }

        //Warp AI to new random position near spawn point
        private void Warp()
        {
            int rnd = Random.Range(1, 3);
            if (rnd == 1)
            {
                transform.position = GameManager.Instance.RandomScreenPosition(GameManager.Instance.aiSpawner);
                rigidBody.velocity = Vector3.zero;
            }
        }

        //Calculate damage when the ship is hit. Taking account of shield strength and striking bullets power
        public void Damage(float firePower, string attacker)
        {
            AiDamage.Invoke();
            if (isShielding)
            {
                GameManager.Instance.aiPlayer.Health -= (int)(firePower / (GameManager.Instance.aiPlayer.Faction.Ship.ShieldPower * GameManager.Instance.aiPlayer.ShieldLvl));
            }
            else
            {
                GameManager.Instance.aiPlayer.Health -= (int)firePower;
            }
            if (GameManager.Instance.aiPlayer.Health <= 0)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                if (GameManager.Instance.aiPermadeath)
                {
                    GameManager.Instance.aiPlayer.Lives--;
                }
                if (GameManager.Instance.aiPlayer.Lives <= 0)
                {
                    GameManager.Instance.GameOver();
                }
                StartCoroutine(DestroyShip());
            }
        }

        //Destroy this ship
        IEnumerator DestroyShip()
        {
            for (int i = 0; i < 3; i++)
            {
                //Spawn new explosion
                GameObject explosionObject = ExplosionPooling.explosionPool.Get();
                SoundManager.Instance.PlayShipExplosion();
                explosionObject.transform.position = transform.position;
                explosionObject.transform.rotation = transform.rotation;
                explosionObject.transform.localScale = new Vector3(i, i, i);
                yield return new WaitForSeconds(0.15f);

                //Return explosion to pool (reset scale)
                explosionObject.transform.localScale = new Vector3(1, 1, 1);
                if (gameObject.activeSelf)
                {
                    ExplosionPooling.explosionPool.Release(explosionObject);
                }
            }
            GameManager.Instance.SpawnAi(GameManager.Instance.deathRespawnTime);
            Destroy(gameObject);
        }


        //Wrap ship to opposite side of the screen when exiting
        private void OnBecameInvisible()
        {
            if (gameObject.activeSelf)
            {
                Vector3 viewPort = Camera.main.WorldToViewportPoint(transform.position);
                Vector3 movePos = transform.position;
                if (viewPort.x > 1 || viewPort.x < 0)
                {
                    movePos.x = -movePos.x;
                }
                if (viewPort.y > 1 || viewPort.y < 0)
                {
                    movePos.z = -movePos.z;
                }
                transform.position = movePos;
            }
        }
    }
}