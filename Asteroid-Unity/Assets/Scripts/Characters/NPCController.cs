using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    [HideInInspector] public Transform spawnPoint;
    [Header("Bullet")]
    private float fireStart = 2f;
    private float fireInterval = 1f;
    private float bulletSpeed = 5f;
    [SerializeField] private SO_Players players; //Drag in the ScriptableObject of players

    private void Start()
    {
        InvokeRepeating("Fire", fireStart, fireInterval);
    }

    private void Fire()
    {
        int rnd = Random.Range(1, players.Players.Count + 1);
        GameObject bullet = BulletPooling.bulletPool[0].Get();
        bullet.transform.position = transform.position;
        bullet.transform.LookAt(GameObject.Find("Player " + rnd).transform);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
    }


    //Remove NPC when it leaves the screen
    private void OnBecameInvisible()
    {
        spawnPoint.GetComponent<NPCSpawner>().isNpcSpawned = false;
        Destroy(this.gameObject);
    }
}
