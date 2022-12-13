using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player player;

    private void Start()
    {
        Instantiate(player.Ship.ShipPrefab);
    }
}
