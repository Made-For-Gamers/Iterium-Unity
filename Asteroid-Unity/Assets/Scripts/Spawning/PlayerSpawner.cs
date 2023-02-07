using UnityEngine;

namespace Iterium
{

    // Spawn player ship prefab

    public class PlayerSpawner : MonoBehaviour
    {
        float spawnTime = 2f;

        private void Start()
        {
            GameManager.Instance.playerSpawner = gameObject.transform;
            GameManager.Instance.SpawnPlayer(spawnTime);
        }
    }
}