using UnityEngine;

/// <summary>
/// Attached to the bullet prefab and handles...
/// * Collision detection
/// * Asteroid splitting
/// * Crystal spawning
/// * Bullet de-spawning
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private SO_GameObjects asteroids;
    [SerializeField] private SO_GameObjects crystals;

    [Header("Chance of crystal drop 1/?")]
    [SerializeField] private int dropChance = 20;

    [HideInInspector] public int PlayerNumber;
    [HideInInspector] public SO_Player player;

    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits an asteroid
            case "Asteroid":
                BulletExplosion(collision);
                player.Score += 50;
                player.Xp += 10;

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
                    int chance = Random.Range(1, dropChance);
                    if (chance == 1)
                    {
                        Instantiate(crystals.GetRandomGameObject(), collision.gameObject.transform.position, Random.rotation);
                    }
                }

                //Remove asteroid
                AsteroidPooling.asteroidPool.Release(collision.gameObject);
                break;

            //Bullet hits a player
            case "Player":
                var player1Hit = collision.transform.GetComponent<PlayerController>();
                player1Hit.BulletHit(player.Ship.Bullet.FirePower, player);
                player.Score += 250;
                player.Xp += 25;
                BulletExplosion(collision);
                break;

            //Bullet hits NPC
            case "NPC":
                player.Score += 500;
                player.Xp += 50;
                BulletExplosion(collision);
                Destroy(collision.gameObject);
                break;
        }
    } 

    //Remove bullet after a collision
    private void BulletExplosion(Collider obj)
    {

        BulletPooling.bulletPool[PlayerNumber].Release(this.gameObject);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
    }


    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool[PlayerNumber].Release(this.gameObject);
    }
}
