using System.Threading.Tasks;
using UnityEngine;

public class DeSpawnExplosion : MonoBehaviour
{
    //Remove explsion after effect is complete
    [Header("destroy time in milliseconds")]
    [SerializeField] private int destroyTime;

    void OnEnable()
    {
        RemoveExplosions();
    }

    private async void RemoveExplosions()
    {
        await Task.Delay(destroyTime);
        if (gameObject.activeSelf)
        {
            ExplosionPooling.explosionPool.Release(this.gameObject);
        }
    }
}

