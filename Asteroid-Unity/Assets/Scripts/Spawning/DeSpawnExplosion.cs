using System.Collections;
using UnityEngine;

public class DeSpawnExplosion : MonoBehaviour
{
    //Remove explsion after effect is complete
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
            ExplosionPooling.explosionPool.Release(this.gameObject);
        }
    }
}

