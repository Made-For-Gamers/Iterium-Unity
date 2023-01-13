using UnityEngine;

public class DeSpawnBullet : MonoBehaviour
{
    //Remove bullet after it leaves the screen
    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            BulletPooling.bulletPoolPlayer.Release(this.gameObject);
        }
    }
}
