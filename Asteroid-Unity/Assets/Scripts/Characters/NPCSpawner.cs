using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn NPC at random interval
/// </summary>
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player npc;
    [SerializeField] int minSpawnTime;
    [SerializeField] int maxSpawnTime;

    private int rnd;
    public bool isNpcSpawned;

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
        ship.GetComponent<Rigidbody>().velocity = new Vector3(5, 0, 0);      
    }
}
