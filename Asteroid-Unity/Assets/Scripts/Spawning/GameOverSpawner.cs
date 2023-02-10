using UnityEngine;

namespace Iterium
{
    //Spawn the players selected ship on the game over scene

    public class GameOverSpawner : MonoBehaviour
    {
        private void Start()
        {
            GameObject ship = Instantiate(GameManager.Instance.player.Faction.Ship.ShipPrefab);
            ship.transform.position = transform.position;
            ship.transform.GetComponent<InputManager>().enabled = false;
            ship.transform.GetComponent<PlayerController>().enabled = false;
            ship.AddComponent<Rotate>();
        }
    }
}