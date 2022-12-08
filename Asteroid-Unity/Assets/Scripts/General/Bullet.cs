using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Destroy bullets when leaving the screen
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
