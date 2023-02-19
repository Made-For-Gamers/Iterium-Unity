using UnityEngine;

namespace Iterium
{    
    // AI bullet that handles...
    // * Collision detection
    // * Bullet de-spawning

    public class BulletAI : BulletBase
    {
        public float firePower;

        //AI bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            var hitObj = collision.GetComponent<IDamage>();
            if (hitObj == null) return;
            BulletExplosion(collision);
            hitObj.Damage(firePower, "ai");
        }

        //Release bullet to pool
        protected override void ReleaseBullet()
        {
            if (gameObject.activeSelf)
            {
                BulletPooling.bulletPoolAi.Release(gameObject);
            }
        }
    }
}
