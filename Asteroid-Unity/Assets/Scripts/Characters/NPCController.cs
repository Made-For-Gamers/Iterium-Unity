using UnityEngine;

public class NPCController : MonoBehaviour
{
    [HideInInspector] public Transform spawnPoint;

    //Remove NPC when it leaves the screen
    private void OnBecameInvisible()
    {
        spawnPoint.GetComponent<NPCSpawner>().isNpcSpawned = false;
        Destroy(this.gameObject);
    }
}
