using System.Collections;
using UnityEngine;

namespace Iterium
{
    //Remove explosion after effect is complete

    public class DeSpawnExplosion : MonoBehaviour
    {
        
        [SerializeField] private float destroyTime = 0.5f;

        void OnEnable()
        {
            StartCoroutine(RemoveExplosions());
        }

        private IEnumerator RemoveExplosions()
        {
            yield return new WaitForSeconds(destroyTime);
            if (gameObject.activeSelf)
            {
                ExplosionPooling.explosionPool.Release(gameObject);
            }
        }
    }
}