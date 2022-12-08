using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] SO_Player player;
    [SerializeField] SO_GameObjects asteroids;
    GameObject explosionObject;

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision: " + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {            
            case "Asteroid":
                gameObject.GetComponent<BoxCollider>().enabled = false;
                player.Score += 100;
               // explosionObject = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
                Destroy(explosionObject, 1.5f);
                Vector3 position = collision.transform.position;
                Vector3 scale = collision.transform.localScale;
                Destroy(collision.gameObject);
                if (scale.x > 0.25)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject spawnedAsteroid = Instantiate(asteroids.Obj[Random.Range(0, asteroids.Obj.Count)], position, transform.rotation);
                        spawnedAsteroid.transform.localScale = new Vector3(scale.x / 2, scale.y / 2, scale.z / 2);
                    }
                }
                Destroy(gameObject);
                break;

            case "P1":
            case "P2":
                player.Score += 1000;
                explosionObject = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
                Destroy(explosionObject, 1.5f);
                collision.transform.position = new Vector3(0, 0, 0);
                Destroy(gameObject);
                break;
        }

    }
}
