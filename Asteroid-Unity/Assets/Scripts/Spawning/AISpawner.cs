using UnityEngine;

namespace Iterium
{
    // Spawn AI ship prefab

    public class AISpawner : MonoBehaviour
    {
        float spawnTime = 5f;

        private void Start()
        {
            GameManager.Instance.aiSpawner = gameObject.transform;
            GameManager.Instance.SpawnAi(spawnTime);
        }
    }
}