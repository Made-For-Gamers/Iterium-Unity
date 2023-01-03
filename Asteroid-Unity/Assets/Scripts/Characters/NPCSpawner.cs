using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn NPC at random interval
/// </summary>
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player npc;
    [Header("Spawning")]
    [SerializeField] int minSpawnTime;
    [SerializeField] int maxSpawnTime;

    [Header("Movement")]
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;

    private int rnd;
    [HideInInspector] public bool isNpcSpawned;

    private void Update()
    {
        if (!isNpcSpawned)
        {
            int rnd = Random.Range(minSpawnTime, maxSpawnTime);
            isNpcSpawned = true;
            StartCoroutine(SpawnNPC());
        }
    }

    IEnumerator SpawnNPC()
    {
        yield return new WaitForSeconds(rnd);
        GameObject ship = Instantiate(npc.Ship.ShipPrefab);      
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.GetComponent<NPCController>().spawnPoint = transform;
        int rndX = Random.Range(minSpeed, maxSpeed);
        int rndZ = Random.Range(minSpeed / 2, maxSpeed / 2);
        ship.GetComponent<Rigidbody>().velocity = new Vector3(rndX, 0, -rndZ);      
    }
}
