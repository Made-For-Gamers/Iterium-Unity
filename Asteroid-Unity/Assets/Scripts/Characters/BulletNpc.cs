using System;
using UnityEngine;

namespace Iterium
{
    /// <summary>
    /// NPC bullet that handles...
    /// * Collision detection
    /// * Bullet de-spawning
    /// </summary>

    public class BulletNpc : BulletBase
    {
        public static event Action<string> BulletHit;

        //NPC bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Hit asteroid
                case "Asteroid":
                    BulletExplosion(collision);
                    AsteroidHit(collision);
                    break;

                //Hit player
                case "Player":
                    BulletHit.Invoke("player");
                    BulletExplosion(collision);
                    break;

                //Hit AI
                case "AI":
                    BulletHit.Invoke("ai");
                    BulletExplosion(collision);
                    break;

                //Hit another bullet
                case "Bullet":
                    if (collision.GetComponent<BulletAI>())
                    {
                        BulletPooling.bulletPoolAi.Release(collision.gameObject);
                    }
                    else
                    {
                        BulletPooling.bulletPoolPlayer.Release(collision.gameObject);
                    }
                    BulletExplosion(collision);
                    break;
            }
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