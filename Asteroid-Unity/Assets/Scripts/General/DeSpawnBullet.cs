using UnityEngine;

public class DeSpawnBullet : MonoBehaviour
{
    //Remove asteroid after it leaves the screen
    private void OnBecameInvisible()
    {
        if (PlayerController.isBulletPooling)
        {
            PlayerController.bulletPool.Release(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
