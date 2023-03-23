using System;
using UnityEngine;

namespace Iterium
{
    public class BossSpawner : MonoBehaviour
    {
        // Spawn Boss prefab
        // Invoke boss spawn event for GameManager

        [Header("Spawning")]
        [SerializeField] int spawnInterval = 120;
        [SerializeField] int spawnStartDelay = 120;
        public static event Action<Vector3> SpawnBoss;

        private void Start()
        {
            InvokeRepeating("BossSpawn", spawnStartDelay, spawnInterval);
        }

        private void BossSpawn()
        {
            SpawnBoss?.Invoke(transform.position);
        }
    }
}
