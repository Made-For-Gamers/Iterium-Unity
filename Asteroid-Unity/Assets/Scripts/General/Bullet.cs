using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SO_GameObjects explosion;
    [SerializeField] SO_Player player;
    [SerializeField] SO_GameObjects asteroids;
    [SerializeField] SO_GameObjects crystals;
    [Header("Chance of crystal drop 1/?")]
    [SerializeField] int dropChance = 25;
    GameObject explosionObject;

    private void OnCollisionEnter(Collision collision)
    {
        //print("Collision: " + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Asteroid":
                gameObject.GetComponent<BoxCollider>().enabled = false;
                player.Score += 100;
                explosionObject = Instantiate(explosion.GetRandomGameObject(), collision.transform.position, collision.transform.rotation);
                Destroy(explosionObject, 1.5f);
                Vector3 position = collision.transform.position;
                Vector3 scale = collision.transform.localScale;

                //Split asteroid if it is larger than a set size
                if (scale.x > 0.25)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject spawnedAsteroid = Instantiate(asteroids.Obj[Random.Range(0, asteroids.Obj.Count)], position, transform.rotation);
                        spawnedAsteroid.transform.localScale = new Vector3(scale.x / 2, scale.y / 2, scale.z / 2);
                    }
                }

                //Random spawn of a crystal
                int chance = Random.Range(1, dropChance);
                if (chance == 1)
                {
                    Instantiate(crystals.GetRandomGameObject(), collision.gameObject.transform.position, Random.rotation);
                }
                //Remove asteroid
                if (AsteroidSpawner.isAsteroidPooling)
                {
                    AsteroidSpawner.asteroidPool.Release(collision.gameObject);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                //Remove bullet
                if (PlayerController.isBulletPooling)
                {
                    PlayerController.bulletPool.Release(this.gameObject);
                    gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    Destroy(this.gameObject);
                }
                break;

            case "P1":
            case "P2":
                player.Score += 1000;
                explosionObject = Instantiate(explosion.GetRandomGameObject(), collision.transform.position, collision.transform.rotation);
                Destroy(explosionObject, 1.5f);
                collision.transform.position = new Vector3(0, 0, 0);
                Destroy(gameObject);
                break;
        }
    }
}
