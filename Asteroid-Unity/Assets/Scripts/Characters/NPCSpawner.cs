using UnityEngine;

/// <summary>
/// Spawn NPC ship prefab
/// Set random position, LookAt target, random speed
/// Spawn interval time
/// </summary>
public class NPCSpawner : MonoBehaviour
{   
    [Header("Spawning")]
    [SerializeField] int spawnInterval = 45;

    [Header("Movement")]
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;
    [SerializeField] Transform target;
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
        ship.transform.LookAt(target);
        speed = Random.Range(minSpeed, maxSpeed);
        ship.GetComponent<Rigidbody>().velocity = ship.transform.forward * speed;
    }
}
