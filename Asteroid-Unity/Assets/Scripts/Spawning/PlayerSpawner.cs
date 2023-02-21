using System;
using UnityEngine;

namespace Iterium
{
    // Spawn player ship prefab

    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnTime = 5f;
        public static event Action<Vector2, float> SpawnPlayer;

        private void Start()
        {
            SpawnPlayer.Invoke(transform.position, spawnTime);
        }
    }
}