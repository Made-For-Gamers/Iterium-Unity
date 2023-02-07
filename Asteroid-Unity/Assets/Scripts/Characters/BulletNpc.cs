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
                    var playerHit = collision.transform.GetComponent<PlayerController>();
                    playerHit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvlUs);
                    BulletExplosion(collision);
                    break;

                //Hit AI
                case "AI":
                    var aiIhit = collision.transform.GetComponent<AIController>();
                    aiIhit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvlUs);
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