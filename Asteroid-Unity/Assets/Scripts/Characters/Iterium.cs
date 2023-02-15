using System;
using UnityEngine;

namespace Iterium
{
    //Collecting of Iterium by player or AI

    public class Iterium : MonoBehaviour
    {
        public static event Action<string> CollectedIterium;

        //Iterium collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Collected by player
                case "Player":
                    CollectedIterium.Invoke("player");
                    Destroy(gameObject);
                    break;

                //Collected by AI
                case "AI":
                    CollectedIterium.Invoke("ai");
                    Destroy(gameObject);
                    break;
            }
        }
    }
}