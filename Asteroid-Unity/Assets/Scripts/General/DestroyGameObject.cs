using UnityEngine;

namespace Iterium
{

    //Destroy a game object this is attached to over a set time.
    public class DestroyGameObject : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 5f;

        void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}