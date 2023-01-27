using UnityEngine;

/// <summary>
/// Attached to NPC bullet prefab and handles...
/// * Collision detection
/// * Bullet de-spawning
/// </summary>

public class BulletNpc : MonoBehaviour
{
    [Header("Bullet Hit SFX")]
    [SerializeField] private int sfxIndex = 1;

    //Take action when bullet hits a specific object
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Bullet hits an asteroid
            case "Asteroid":
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

                    SoundManager.Instance.PlayAsteroidExplosion();

                    //Random spawn of a crystal
                    int chance = Random.Range(1, GameManager.Instance.iteriumChance);
                    if (chance == 1)
                    {
                        Vector3 pos = new Vector3(collision.gameObject.transform.position.x, 0, collision.gameObject.transform.position.z);
                        Instantiate(GameManager.Instance.iterium.GetRandomGameObject(), pos, Random.rotation);
                    }
                }

                //Remove objects
                if (gameObject.activeSelf)
                {
                    AsteroidPooling.asteroidPool.Release(collision.gameObject);
                }
                BulletExplosion(collision);
                break;

            //Bullet hits player
            case "Player":
                var playerHit = collision.transform.GetComponent<PlayerController>();
                playerHit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvl);
                BulletExplosion(collision);
                break;

            //Bullet hits AI player
            case "AI":
                var aiIhit = collision.transform.GetComponent<AIController>();
                aiIhit.BulletHit(GameManager.Instance.npcPlayer.Character.Ship.Bullet.FirePower * GameManager.Instance.npcPlayer.BulletLvl);
                BulletExplosion(collision);
                break;
            //Bullet hits another bullet
            case "Bullet":
                Destroy(collision.gameObject);
                BulletExplosion(collision);
                break;
        }
    }

    //Return bullet to pool after a collision, explosion effect
    private void BulletExplosion(Collider obj)
    {
        SoundManager.Instance.PlayEffect(sfxIndex);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolNpc.Release(this.gameObject);
        }
    }

    //Return bullet to pool after it leaves the screen
    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolNpc.Release(this.gameObject);
        }
    }
}
