using UnityEngine;

// Spawn player ship prefab

public class PlayerSpawner : MonoBehaviour
{
    float spawnTime = 2f;

    private void Start()
    {
        GameManager.Instance.SpawnPlayer(spawnTime);
    }
}
