using System.Collections;
using UnityEngine;

public class DeSpawnExplosion : MonoBehaviour
{
    //Remove explsion after effect is complete
    [SerializeField] private float destroyTime;

    void OnEnable()
    {
        StartCoroutine(RemoveExplosions());
    }

    IEnumerator RemoveExplosions()
    {
        yield return new WaitForSeconds(destroyTime);
        ExplosionPooling.explosionPool.Release(this.gameObject);
    }
}

