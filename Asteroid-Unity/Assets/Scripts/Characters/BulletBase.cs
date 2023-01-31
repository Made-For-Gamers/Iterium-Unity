using UnityEngine;

/// <summary>
/// Bullet base class that handles...
/// * Asteroid splitting 
/// * Crystal spawning 
/// * Bullet explosion
/// </summary>

public abstract class BulletBase : MonoBehaviour
{
    [Header("Bullet Hit SFX")]
    [SerializeField] protected int sfxIndex = 1;

    //Split asteroid when hit
    protected void AsteroidHit(Collider collision)
    {
        //Split asteroid if larger than a set size
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

        //Release asteroid to pool
        if (collision.gameObject.activeSelf)
        {
            AsteroidPooling.asteroidPool.Release(collision.gameObject);
        }
    }

    //Release bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        ReleaseBullet();
    }

    //Release bullet to pool
    protected void BulletExplosion(Collider obj)
    {
        SoundManager.Instance.PlayEffect(sfxIndex);
        GameObject explosionObject = ExplosionPooling.explosionPool.Get();
        explosionObject.transform.position = obj.transform.position;
        explosionObject.transform.rotation = obj.transform.rotation;
        ReleaseBullet();
    }

    protected virtual void ReleaseBullet() { }
}
