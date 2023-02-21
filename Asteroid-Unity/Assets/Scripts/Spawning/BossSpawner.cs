using System;
using UnityEngine;

namespace Iterium
{
    public class BossSpawner : MonoBehaviour
    {
        // Spawn Boss prefab
        // Invoke boss spawn event for GameManager

        [Header("Spawning")]
        [SerializeField] int spawnInterval = 5;
        public static event Action SpawnBoss;

        private void Start()
        {
            InvokeRepeating("Bossspawn", spawnInterval, spawnInterval);
        }

        private void BossSpawn()
        {
            SpawnBoss.Invoke();
        }
    }
}
