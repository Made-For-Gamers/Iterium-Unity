using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{  

    private void Start()
    {
        GameObject ship = Instantiate(Singleton.Instance.player.Character.Ship.ShipPrefab);
        ship.transform.position = transform.position;
        ship.transform.rotation = transform.rotation;
        ship.transform.name = "Player";
        ship.GetComponent<PlayerController>().spawnPoint = transform;
    }
}
