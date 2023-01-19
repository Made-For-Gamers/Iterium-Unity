using UnityEngine;

// Spawn AI ship prefab

public class AISpawner : MonoBehaviour
{
    float spawnTime = 2f;

    private void Start()
    {
        GameManager.Instance.SpawnAi(spawnTime);
    }
}
