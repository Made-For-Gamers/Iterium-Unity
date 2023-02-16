using System;
using UnityEngine;

namespace Iterium
{
    /// <summary>
    /// Player bullet that handles...
    /// * Collision detection
    /// * Asteroid splitting (base class)
    /// * Crystal spawning (base class)
    /// * Bullet de-spawning
    /// </summary>

    public class Bullet : BulletBase
    {
        public static event Action<string> BulletHit;

        //Player bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Hit asteroid
                case "Asteroid":
                    BulletHit.Invoke("asteroid");
                    BulletExplosion(collision);
                    AsteroidHit(collision);
                    break;

                //Hit player
                case "AI":
                    BulletHit.Invoke("ai");
                    BulletExplosion(collision);
                    break;

                //Hit NPC
                case "NPC":
                    BulletHit.Invoke("npc");
                    BulletExplosion(collision);
                    Destroy(collision.gameObject);
                    break;

            }
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