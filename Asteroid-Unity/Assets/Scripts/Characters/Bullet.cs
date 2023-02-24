using UnityEngine;

namespace Iterium
{    
    // Player bullet that handles...
    // * Collision detection
    // * Bullet de-spawning

    public class Bullet : BulletBase
    {
        [HideInInspector] public float firePower;

        //Player bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            var hitObj = collision.GetComponent<IDamage>();
            if (hitObj == null) return;
            BulletExplosion(collision);
            hitObj.Damage(firePower, "player");
        }

        //Release bullet to pool
        protected override void ReleaseBullet()
        {
            if (gameObject.activeSelf)
            {
                BulletPooling.bulletPoolPlayer.Release(gameObject);
            }
        }
    }
}
