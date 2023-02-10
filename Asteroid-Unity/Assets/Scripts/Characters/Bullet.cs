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
        //Player bullet collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Hit asteroid
                case "Asteroid":
                    GameManager.Instance.player.Score += 50;
                    GameManager.Instance.player.XpCollected += 10;
                    BulletExplosion(collision);
                    AsteroidHit(collision);
                    break;

                //Hit player
                case "AI":
                    var aiIhit = collision.transform.GetComponent<AIController>();
                    aiIhit.BulletHit(GameManager.Instance.player.Faction.Ship.Bullet.FirePower * GameManager.Instance.player.BulletLvl);
                    GameManager.Instance.player.Score += 500;
                    GameManager.Instance.player.XpCollected += 25;
                    BulletExplosion(collision);
                    break;

                //Hit NPC
                case "NPC":
                    GameManager.Instance.player.Score += 2500;
                    GameManager.Instance.player.XpCollected += 100;
                    SoundManager.Instance.PlayShipExplosion();
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