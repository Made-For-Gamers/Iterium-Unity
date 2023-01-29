using UnityEngine;

/// <summary>
/// NPC bullet that handles...
/// * Collision detection
/// * Bullet de-spawning
/// </summary>

public class BulletNpc : BulletBase
{
    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits an asteroid
            case "Asteroid":
                AsteroidHit(collision);
                break;

            //Bullet hits player
            case "Player":
                var playerHit = collision.transform.GetComponent<PlayerController>();
                playerHit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvl);
                break;

            //Bullet hits AI player
            case "AI":
                var aiIhit = collision.transform.GetComponent<AIController>();
                aiIhit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvl);
                break;

            //Bullet hits another bullet
            case "Bullet":
                if (collision.GetComponent<BulletAI>())
                {
                    BulletPooling.bulletPoolAi.Release(collision.gameObject);
                }
                else
                {
                    BulletPooling.bulletPoolPlayer.Release(collision.gameObject);
                }
                break;
        }
        //if (collision.gameObject != this.gameObject)
        //{
        //    BulletExplosion(collision);
        //}
    }

    protected override void ReleaseBullet()
    {
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolNpc.Release(this.gameObject);
        }
    }
}
