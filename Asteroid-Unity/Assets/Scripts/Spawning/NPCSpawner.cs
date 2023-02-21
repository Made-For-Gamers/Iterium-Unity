using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    // Spawn NPC ship prefab
    // Random spawn point position
    // Invoke spawn event for GameManager

    public class NPCSpawner : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] int spawnInterval = 12;
        public static event Action<Vector3> SpawnNpc;
        private Vector3 spawnPosition;

        private void Start()
        {
            InvokeRepeating("SpawnNPC", spawnInterval, spawnInterval);
        }

        private void SpawnNPC()
        {
            int rnd = Random.Range(1, 5);
            spawnPosition = GetComponentsInChildren<Transform>()[rnd].position;
            SpawnNpc.Invoke(spawnPosition);
        }
    }
}