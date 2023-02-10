using UnityEngine;
using System.Collections;

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

    public class AIController : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] private float fireStart = 2f;
        [SerializeField] private float fireInterval = 0.6f;
        [SerializeField] private int decisionCycle = 3; //Number of bullets to fire in a targeting descition round

        private Transform firePosition;
        private GameObject shield;
        private float shieldCooldown;
        private bool isShielding;
        private GameObject thrusters;
        private bool isThrusting;
        private Rigidbody rigidBody;
        private int shots;
        private bool attackNPC;

        private void Start()
        {
            shield = transform.GetChild(0).gameObject;
            thrusters = transform.GetChild(1).gameObject;
            firePosition = transform.GetChild(2);
            rigidBody = GetComponent<Rigidbody>();
            InvokeRepeating("Fire", fireStart, fireInterval);
            InvokeRepeating("Shield", 0, GameManager.Instance.aiPlayer.Faction.Ship.ShieldCooldown);
        }

        private void Update()
        {
            Rotate();
        }

        private void FixedUpdate()
        {
            Thrust();
        }

        //AI Ship targeting, either the player or NPC
        private void Rotate()
        {
            if (attackNPC && GameManager.Instance.targetNpc.gameObject)
            {
                transform.LookAt(GameManager.Instance.targetNpc.transform);
            }
            else if (GameManager.Instance.targetPlayer.gameObject)
            {
                transform.LookAt(GameManager.Instance.targetPlayer.transform);
            }
        }

        //Firing
        private void Fire()
        {
            if (!GameManager.Instance.targetNpc.gameObject)
            {
                attackNPC = false;
            }
            if (GameManager.Instance.targetPlayer.gameObject || GameManager.Instance.targetNpc.gameObject)
            {
                GameObject bullet = BulletPooling.bulletPoolAi.Get();
                bullet.transform.position = firePosition.position;

                //New descition round - attack NPC or not?
                if (attackNPC == false && shots == 0 && GameManager.Instance.targetNpc.gameObject)
                {
                    int rnd = Random.Range(1, 3);
                    if (rnd == 1)
                    {
                        attackNPC = true;
                    }
                }

                if (attackNPC) //Point bullet towards NPC, else the player
                {
                    bullet.transform.LookAt(GameManager.Instance.targetNpc.transform);
                }
                else if (GameManager.Instance.targetPlayer.gameObject) 
                {
                    bullet.transform.LookAt(GameManager.Instance.targetPlayer.transform);
                }
                else
                {
                    bullet.transform.Rotate(Vector3.zero);
                }

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * (GameManager.Instance.aiPlayer.Faction.Ship.Bullet.Speed * GameManager.Instance.aiPlayer.BulletLvl);
                shots++;

                if (shots >= decisionCycle)
                {
                    //reset descition round
                    shots = 0;
                    if (attackNPC)
                    {
                        attackNPC = false;
                    }
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
                rigidBody.AddRelativeForce(new Vector3(0, 0, 0.2f * (GameManager.Instance.aiPlayer.Faction.Ship.Thrust * GameManager.Instance.aiPlayer.SpeedLvl) * Time.deltaTime), ForceMode.Force);
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

        //Calculate damage when the ship is hit. Taking account of shield strength and striking bullets power
        public void BulletHit(float firePower)
        {
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