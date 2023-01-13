using UnityEngine;

/// <summary>
/// Player bullet that handles...
/// * Collision detection
/// * Asteroid splitting
/// * Crystal spawning
/// * Bullet de-spawning
/// </summary>
public class Bullet : MonoBehaviour
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

                //Split asteroid if it is larger than a set size
                Vector3 scale = collision.transform.localScale;
                if (scale.x > 0.25)
                {
                    int rnd = Random.Range(2, 5); //Split into random number of pieces
                    for (int i = 0; i < rnd; i++)
                    {
                        GameObject spawnedAsteroid = AsteroidPooling.asteroidPool.Get();
                        spawnedAsteroid.transform.rotation = collision.transform.rotation;
                        spawnedAsteroid.transform.localScale = new Vector3(scale.x / rnd, scale.y / rnd, scale.z / rnd);
                        spawnedAsteroid.transform.position = new Vector3(collision.transform.position.x, 0, collision.transform.position.z);
                        spawnedAsteroid.GetComponent<Rigidbody>().mass = collision.transform.GetComponent<Rigidbody>().mass / rnd;
                    }

                    //Random spawn of a crystal
                    int chance = Random.Range(1, GameManager.Instance.iteriumChance);
                    if (chance == 1)
                    {
                        Instantiate(GameManager.Instance.crystals.GetRandomGameObject(), collision.gameObject.transform.position, Random.rotation);
                    }
                }

                //Remove objects
                if (gameObject.activeSelf)
                {
                    AsteroidPooling.asteroidPool.Release(collision.gameObject);
                }
                BulletExplosion(collision);
                break;

            //Bullet hits AI player
            case "AI":
                var aiIhit = collision.transform.GetComponent<AIController>();
                aiIhit.BulletHit(GameManager.Instance.player.Character.Ship.Bullet.FirePower);
                GameManager.Instance.player.Score += 500;
                GameManager.Instance.player.Xp += 25;
                BulletExplosion(collision);
                break;

            //Bullet hits NPC
            case "NPC":
                GameManager.Instance.player.Score += 2500;
                GameManager.Instance.player.Xp += 100;
                BulletExplosion(collision);
                Destroy(collision.gameObject);
                break;
        }
    } 

    //Remove bullet after a collision
    private void BulletExplosion(Collider obj)
    {
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolPlayer.Release(this.gameObject);
        }
    }

    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolPlayer.Release(this.gameObject);
        }
    }

  
}
