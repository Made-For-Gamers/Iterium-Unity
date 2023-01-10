using UnityEngine;

public class AISpawner : MonoBehaviour
{  

    private void Start()
    {
        GameObject ship = Instantiate(GameManager.Instance.aiCharacter.Ship.ShipPrefab);
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.transform.name = "AI";
        //ship.GetComponent<PlayerController>().spawnPoint = transform;
    }
}
