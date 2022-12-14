using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SO_Player player;

    private void Start()
    {
       GameObject ship = Instantiate(player.Ship.ShipPrefab);
        ship.transform.position = transform.position;
        if (player.Player2)
        {
            ship.transform.tag = "Player2";
        }
        else
        {
            ship.transform.tag = "Player1";
        }
    }
}
