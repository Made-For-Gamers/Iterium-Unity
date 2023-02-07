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
        //AI bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Hit asteroid
                case "Asteroid":
                    GameManager.Instance.aiPlayer.Score += 50;
                    GameManager.Instance.aiPlayer.XpCollected += 10;
                    BulletExplosion(collision);
                    AsteroidHit(collision);
                    break;

                //Hit player
                case "Player":
                    var playerhit = collision.transform.GetComponent<PlayerController>();
                    playerhit.BulletHit(GameManager.Instance.aiPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.aiPlayer.BulletLvlUs);
                    GameManager.Instance.aiPlayer.Score += 500;
                    GameManager.Instance.aiPlayer.XpCollected += 25;
                    BulletExplosion(collision);
                    break;

                //Hit NPC
                case "NPC":
                    GameManager.Instance.aiPlayer.Score += 2500;
                    GameManager.Instance.aiPlayer.XpCollected += 100;
                    SoundManager.Instance.PlayShipExplosion();
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