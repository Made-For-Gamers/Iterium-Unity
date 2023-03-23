using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iterium
{
    public class Asteroid : MonoBehaviour, IDamage
    {
        [Header("Iterium Drop ?:1")] 
        [SerializeField] private int iteriumChance = 60;
        
        //Events
        public static event Action<string> AsteroidDamage;
        public static event Action<Vector3> DropIterium;
       

        public void Damage(float firePower, string attacker)
        {
            AsteroidDamage?.Invoke(attacker);
            //Split if larger than a quarter of its size
            Vector3 scale = transform.localScale;
            if (scale.x > 0.25)
            {
                //Split into random number of pieces
                int rnd = Random.Range(2, 5);
                for (int i = 0; i < rnd; i++)
                {
                    GameObject spawnedAsteroid = AsteroidPooling.asteroidPool.Get();
                    spawnedAsteroid.transform.rotation = transform.rotation;
                    spawnedAsteroid.transform.localScale = new Vector3(scale.x / rnd, scale.y / rnd, scale.z / rnd);
                    spawnedAsteroid.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    spawnedAsteroid.GetComponent<Rigidbody>().mass = transform.GetComponent<Rigidbody>().mass / rnd;
                }

                //Random spawn of a crystal
                int chance = Random.Range(1, iteriumChance);
                if (chance == 1)
                {
                    Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
                    DropIterium?.Invoke(pos);
                }
            }

            //Release asteroid to pool
            if (gameObject.activeSelf)
            {
                AsteroidPooling.asteroidPool.Release(gameObject);
            }
        }

        //Remove asteroid after it leaves the screen
        private void OnBecameInvisible()
        {
            if (gameObject.activeSelf)
            {
                AsteroidPooling.asteroidPool.Release(gameObject);
            }
        }
    }
}

