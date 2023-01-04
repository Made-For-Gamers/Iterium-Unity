using UnityEngine;

/// <summary>
/// Spawn NPC at random interval
/// </summary>
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player npc;
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
        GameObject ship = Instantiate(npc.Ship.ShipPrefab);  
        ship.transform.position = GetComponentsInChildren<Transform>()[rnd].position;
        ship.transform.LookAt(target);       
        speed = Random.Range(minSpeed, maxSpeed);      
        ship.GetComponent<Rigidbody>().velocity = ship.transform.forward * speed;      
    }
}