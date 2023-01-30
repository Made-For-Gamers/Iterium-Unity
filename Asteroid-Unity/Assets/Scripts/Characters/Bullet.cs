using UnityEngine;

/// <summary>
/// Player bullet that handles...
/// * Collision detection
/// * Asteroid splitting (base class)
/// * Crystal spawning (base class)
/// * Bullet de-spawning
/// </summary>

public class Bullet : BulletBase
{
    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits an asteroid
            case "Asteroid":
                GameManager.Instance.player.Score += 50;
                GameManager.Instance.player.Xp += 10;
                BulletExplosion(collision);
                AsteroidHit(collision);
                break;

            //Bullet hits AI player
            case "AI":
                var aiIhit = collision.transform.GetComponent<AIController>();
                aiIhit.BulletHit(GameManager.Instance.player.Character.Ship.Bullet.FirePower * GameManager.Instance.player.BulletLvl);
                GameManager.Instance.player.Score += 500;
                GameManager.Instance.player.Xp += 25;
                BulletExplosion(collision);
                break;

            //Bullet hits NPC
            case "NPC":
                GameManager.Instance.player.Score += 2500;
                GameManager.Instance.player.Xp += 100;
                SoundManager.Instance.PlayShipExplosion();
                BulletExplosion(collision);
                Destroy(collision.gameObject);
                break;
        }
    }

    protected override void ReleaseBullet()
    { 
     if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolPlayer.Release(this.gameObject);
        }
    }
}
