using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Iterium
{
    //Spawn the players selected ship on the game over scene
    //Disable attached scripts and thrust, shield gameObjects

    public class GameOverSpawner : MonoBehaviour
    {
        private void Start()
        {
            GameObject ship = Instantiate(GameManager.Instance.player.Faction.Ship.ShipPrefab);
            ship.transform.position = transform.position;
            ship.transform.GetComponent<InputManager>().enabled = false;
            ship.transform.GetComponent<PlayerController>().enabled = false;
            ship.transform.GetChild(0).gameObject.SetActive(false);
            ship.transform.GetChild(1).gameObject.SetActive(false);
            ship.AddComponent<Rotate>();
        }
    }
}