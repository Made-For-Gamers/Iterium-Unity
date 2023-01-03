using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{   
    [Header("Bullet")]
    [SerializeField] private float fireStart = 3f;
    [SerializeField] private float fireInterval = 1.5f;

    [Header("Characters")]
    [SerializeField] private SO_Players players; //Drag in the ScriptableObject list of players
    [SerializeField] private SO_Players npcs; //Drag in the ScriptableObject list of NPCs

    private void Start()
    {
        InvokeRepeating("Fire", fireStart, fireInterval);
    }

    private void Fire()
    {        
        int rnd = Random.Range(1, players.Players.Count + 1);
        GameObject bullet = BulletPooling.bulletPool[0].Get();
        Vector3 position = transform.position;
        position.x = position.x + transform.localScale.x + bullet.transform.localScale.x;
        bullet.transform.position = transform.position ;
        bullet.transform.LookAt(GameObject.Find("Player " + rnd).transform);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * npcs.Players[0].Ship.Bullet.Speed;

        //Increase NPC velocity if ship speed becomes too slow due to collision with asteroids
        if (transform.GetComponent<Rigidbody>().velocity.x <= 2f)
        {
            transform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6,7), 0, Random.Range(-6, 7));
        }
    }

    //Remove NPC when leaving the screen
    private void OnBecameInvisible()
    {       
        Destroy(this.gameObject);
    }
}
