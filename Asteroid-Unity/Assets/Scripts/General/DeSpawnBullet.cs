using UnityEngine;

public class DeSpawnBullet : MonoBehaviour
{
    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool.Release(this.gameObject);
    }
}
