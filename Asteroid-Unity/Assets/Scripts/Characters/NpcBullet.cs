using UnityEngine;

/// <summary>
/// Attached to NPC bullet prefab and handles...
/// * Collision detection
/// * Bullet de-spawning
/// </summary>

public class NpcBullet : MonoBehaviour
{    

    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
          switch (collision.gameObject.tag)
        {
            //NPC Bullet hits a player
            case "Player":
                var playerHit = collision.transform.GetComponent<PlayerController>();
                playerHit.BulletHit(Singleton.Instance.npc.Character.Ship.Bullet.FirePower);              
                BulletExplosion(collision);
                break;          
        }
    }  

    //Return bullet to pool after a collision, explosion effect
    private void BulletExplosion(Collider obj)
    {

        BulletPooling.bulletPoolNpc.Release(this.gameObject);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
    }

    //Return bullet to pool after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPoolNpc.Release(this.gameObject);
    }
}
