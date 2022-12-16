using UnityEngine;

public class DeSpawnBullet : MonoBehaviour
{
    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        BulletPooling.bulletPool[transform.GetComponent<Bullet>().PlayerNumber-1].Release(this.gameObject);
    }
}
