using UnityEngine;

/// <summary>
/// Attached to NPC bullet prefab and handles...
/// * Collision detection
/// * Bullet de-spawning
/// </summary>

public class NpcBullet : MonoBehaviour
{  
    [SerializeField] private SO_Players npcs;

    //Take action when bullet hits a specific object
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits a player
            case "Player":
                var player1Hit = collision.transform.GetComponent<PlayerController>();
                player1Hit.BulletHit(npcs.Players[0].Ship.Bullet.FirePower, null);              
                BulletExplosion(collision);
                break;          
        }
    }

    //Remove bullet after a collision
    private void BulletExplosion(Collision obj)
    {

        BulletPooling.bulletPool[0].Release(this.gameObject);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
    }


    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool[0].Release(this.gameObject);
    }
}
