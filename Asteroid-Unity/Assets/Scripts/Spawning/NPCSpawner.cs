using UnityEngine;

namespace Iterium
{
    // Spawn NPC ship prefab, setting...
    // Random position
    // LookAt target
    // Random speed
    // Spawn interval time

    public class NPCSpawner : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] int spawnInterval = 12;

        [Header("Movement")]
        [SerializeField] int minSpeed = 3;
        [SerializeField] int maxSpeed = 7;
        private int speed;

        private void Start()
        {
            InvokeRepeating("SpawnNPC", spawnInterval, spawnInterval);
        }

        private void SpawnNPC()
        {
            int rnd = Random.Range(1, 5);
            GameObject ship = Instantiate(GameManager.Instance.npcPlayer.Faction.Ship.ShipPrefab);
            ship.transform.position = GetComponentsInChildren<Transform>()[rnd].position;

            //Target player first, or then AI, or then center of screen
            if (GameManager.Instance.targetPlayer.gameObject)
            {
                ship.transform.LookAt(GameManager.Instance.targetPlayer.transform);
            }
            else if (GameManager.Instance.targetAi.gameObject)
            {
                ship.transform.LookAt(GameManager.Instance.targetAi.transform);
            }
            else
            {
                ship.transform.Rotate(Vector3.zero);
            }

            speed = Random.Range(minSpeed, maxSpeed);
            ship.GetComponent<Rigidbody>().velocity = ship.transform.forward * speed;
            GameManager.Instance.targetNpc = ship;
        }
    }
}