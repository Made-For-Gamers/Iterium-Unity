using UnityEngine;

/// <summary>
/// AI bullet that handles...
/// * Collision detection
/// * Asteroid splitting (base class)
/// * Crystal spawning (base class)
/// * Bullet de-spawning
/// </summary>

public class BulletAI : BulletBase
{
    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits an asteroid
            case "Asteroid":
                GameManager.Instance.aiPlayer.Score += 50;
                GameManager.Instance.aiPlayer.XpCollected += 10;
                BulletExplosion(collision);
                AsteroidHit(collision);
                break;

            //Bullet hits another player
            case "Player":
                var playerhit = collision.transform.GetComponent<PlayerController>();
                playerhit.BulletHit(GameManager.Instance.aiPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.aiPlayer.BulletLvlUs);
                GameManager.Instance.aiPlayer.Score += 500;
                GameManager.Instance.aiPlayer.XpCollected += 25;
                BulletExplosion(collision);
                break;

            //Bullet hits NPC
            case "NPC":
                GameManager.Instance.aiPlayer.Score += 2500;
                GameManager.Instance.aiPlayer.XpCollected += 100;
                SoundManager.Instance.PlayShipExplosion();
                BulletExplosion(collision);
                Destroy(collision.gameObject);
                break;
            //Bullet hits another bullet
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

    protected override void ReleaseBullet()
    {
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolAi.Release(this.gameObject);
        }
    }
}
