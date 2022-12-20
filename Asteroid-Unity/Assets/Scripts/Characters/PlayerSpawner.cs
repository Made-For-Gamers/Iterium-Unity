using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player player;
    [SerializeField] private int playerNumber;

    private void Start()
    {
        GameObject ship = Instantiate(player.Ship.ShipPrefab);
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.GetComponent<PlayerController>().playerNumber = playerNumber;
        ship.GetComponent<PlayerController>().spawnPoint = transform;
    }
}
