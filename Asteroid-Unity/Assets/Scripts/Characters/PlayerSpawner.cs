using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player player;   

    private void Start()
    {
        GameObject ship = Instantiate(player.Ship.ShipPrefab);       
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.transform.name = "Player";
        ship.GetComponent<PlayerController>().spawnPoint = transform;       
    }
}
