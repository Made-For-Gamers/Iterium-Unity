using UnityEngine;

namespace Iterium
{
    // NPC bullet that handles...
    // * Collision detection
    // * Bullet de-spawning

    public class BulletNpc : BulletBase
    {
        //NPC bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.tag == "NPC") return;
            var hitObj = collision.GetComponent<IDamage>();
            if (hitObj == null) return;
            BulletExplosion(collision);
            hitObj.Damage(50, "npc");
        }

        //Release bullet to pool
        protected override void ReleaseBullet()
        {
            if (gameObject.activeSelf)
            {
                BulletPooling.bulletPoolNpc.Release(gameObject);
            }
        }
    }
}