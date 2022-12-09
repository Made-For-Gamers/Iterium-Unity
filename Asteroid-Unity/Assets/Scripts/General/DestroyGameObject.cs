using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }
   
}
