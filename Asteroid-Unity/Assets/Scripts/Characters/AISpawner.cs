using UnityEngine;

public class AISpawner : MonoBehaviour
{  

    private void Start()
    {
        GameObject ship = Instantiate(GameManager.Instance.aiPlayer.Character.Ship.ShipPrefab);
        Destroy(ship.GetComponent<PlayerController>());
        Destroy(ship.GetComponent<InputManager>());
        ship.AddComponent<AIController>();
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.transform.name = "AI";
        ship.transform.tag = "AI";
        
        ship.GetComponent<AIController>().spawnPoint = transform;
    }
}
