using UnityEngine;

public class DeSpawn : MonoBehaviour
{
    //Destroy GameObject after leaving the screen
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
