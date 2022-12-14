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
    [SerializeField] SO_Player player;
    [SerializeField] SO_GameObjects asteroids;
    [SerializeField] SO_GameObjects crystals;
    [Header("Chance of crystal drop 1/?")]
    [SerializeField] int dropChance = 20;   

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Asteroid":

                BulletHit(collision);
                player.Score += 100;
                player.Xp += 1;

                //Split asteroid if it is larger than a set size
                Vector3 scale = collision.transform.localScale;
                if (scale.x > 0.25)
                {
                    int rnd = Random.Range(2, 5);
                    for (int i = 0; i < rnd; i++)
                    {
                        GameObject spawnedAsteroid = AsteroidPooling.asteroidPool.Get();
                        spawnedAsteroid.transform.rotation = collision.transform.rotation;
                        spawnedAsteroid.transform.localScale = new Vector3(scale.x / rnd, scale.y / rnd, scale.z / rnd);
                        spawnedAsteroid.transform.position = new Vector3(collision.transform.position.x, 0 , collision.transform.position.z);
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

            case "Player1":
                var player1Hit = collision.transform.GetComponent<PlayerController>().player.Ship;
                player1Hit.BulletHit(player1Hit.Bullet.FirePower, player1Hit.ShieldPower);
                break;
            case "Player2":
                var player2Hit = collision.transform.GetComponent<PlayerController>().player.Ship;
                player2Hit.BulletHit(player2Hit.Bullet.FirePower, player2Hit.ShieldPower);
                break;
        }
    }

    private void BulletHit(Collision obj)
    {
        //Remove bullet
        BulletPooling.bulletPool.Release(this.gameObject);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
    }


    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool.Release(this.gameObject);
    }
}
