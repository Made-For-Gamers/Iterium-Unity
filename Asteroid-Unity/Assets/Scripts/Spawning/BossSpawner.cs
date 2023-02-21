using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    public class BossSpawner : MonoBehaviour
    {
        // Spawn Boss prefab
        // Invoke boss spawn event for GameManager

        public class NPCSpawner : MonoBehaviour
        {
            [Header("Spawning")]
            [SerializeField] int spawnInterval = 5;
            public static event Action<Vector3> SpawnBoss;

            private void Start()
            {
                InvokeRepeating("SpawnNPC", spawnInterval, spawnInterval);
            }

            private void SpawnNPC()
            {
                SpawnBoss.Invoke(transform.position);
            }
        }
    }
}
