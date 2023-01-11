using UnityEngine;

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
