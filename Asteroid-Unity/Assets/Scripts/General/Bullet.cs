using UnityEngine;

/// <summary>
/// Attached to the bullet prefab and handles...
/// Collision detection
/// Asteroid splitting
/// Crystal spawning
/// Bullet de-spawning
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

                //Remove bullet
                BulletPooling.bulletPool.Release(this.gameObject);

                player.Score += 100;
                GameObject explosionObject = ExplosionPooling.explosionPool.Get();
                explosionObject.transform.position = collision.transform.position;
                explosionObject.transform.rotation = collision.transform.rotation;

                //Split asteroid if it is larger than a set size
                Vector3 scale = collision.transform.localScale;
                if (scale.x > 0.25)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject spawnedAsteroid = AsteroidPooling.asteroidPool.Get();
                        spawnedAsteroid.transform.rotation = collision.transform.rotation;
                        spawnedAsteroid.transform.localScale = new Vector3(scale.x / 2, scale.y / 2, scale.z / 2);
                        spawnedAsteroid.transform.position = collision.transform.position;
                        spawnedAsteroid.GetComponent<Rigidbody>().mass = collision.transform.GetComponent<Rigidbody>().mass / 2;
                    }
                }

                //Random spawn of a crystal
                int chance = Random.Range(1, dropChance);
                if (chance == 1)
                {
                    Instantiate(crystals.GetRandomGameObject(), collision.gameObject.transform.position, Random.rotation);
                }

                //Remove asteroid
                AsteroidPooling.asteroidPool.Release(collision.gameObject);
                break;

            case "P1":
            case "P2":
                break;
        }
    }


    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool.Release(this.gameObject);
    }
}
