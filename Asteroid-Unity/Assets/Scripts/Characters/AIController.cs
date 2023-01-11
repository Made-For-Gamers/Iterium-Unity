using UnityEngine;

public class AIController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform);
    }
}
