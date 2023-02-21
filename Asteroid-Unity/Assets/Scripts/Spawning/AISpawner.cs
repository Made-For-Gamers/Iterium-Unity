using System;
using UnityEngine;

namespace Iterium
{
    // Spawn AI ship prefab

    public class AISpawner : MonoBehaviour
    {
        [SerializeField] private float spawnTime = 5f;
        public static event Action<Vector2, float> SpawnAi;

        private void Start()
        {
            SpawnAi.Invoke(transform.position, spawnTime);
        }
    }
}