using UnityEngine;

/// <summary>
/// Spawn NPC ship prefab
/// Set random position, LookAt target, random speed
/// Spawn interval time
/// </summary>
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
        GameObject ship = Instantiate(GameManager.Instance.npcPlayer.Character.Ship.ShipPrefab);
        ship.transform.position = GetComponentsInChildren<Transform>()[rnd].position;
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
