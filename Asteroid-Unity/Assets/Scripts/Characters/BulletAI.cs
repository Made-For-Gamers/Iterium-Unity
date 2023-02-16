using System;
using UnityEngine;

namespace Iterium
{
    /// <summary>
    /// AI bullet that handles...
    /// * Collision detection
    /// * Asteroid splitting (base class)
    /// * Crystal spawning (base class)
    /// * Bullet de-spawning
    /// </summary>

    public class BulletAI : BulletBase
    {
        public static event Action<string> BulletHit;

        //AI bullet collision
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
                case "Player":
                    BulletHit.Invoke("player");
                    BulletExplosion(collision);
                    break;

                //Hit NPC
                case "NPC":
                    BulletHit.Invoke("npc");
                    BulletExplosion(collision);
                    Destroy(collision.gameObject);
                    break;
                //Hit another bullet
                case "Bullet":
                    if (collision.GetComponent<BulletNpc>())
                    {
                        BulletPooling.bulletPoolNpc.Release(collision.gameObject);
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
                BulletPooling.bulletPoolAi.Release(gameObject);
            }
        }
    }
}