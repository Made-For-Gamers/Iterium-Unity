using UnityEngine;

/// <summary>
/// Spawn player ship prefab
/// Set position and rotation
/// Set name and tag
/// Set the spawnpoint in the player controller for re-spawns (death/resets)
/// </summary>

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        GameObject ship = Instantiate(GameManager.Instance.player.Character.Ship.ShipPrefab);
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.transform.name = "Player";
        ship.transform.tag = "Player";
        ship.GetComponent<PlayerController>().spawnPoint = transform;
    }
}
