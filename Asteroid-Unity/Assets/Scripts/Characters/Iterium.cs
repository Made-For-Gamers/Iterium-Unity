using System;
using UnityEngine;

namespace Iterium
{
    //Collecting of Iterium by player or AI

    public class Iterium : MonoBehaviour
    {
        //Events
        public static event Action<string> CollectIterium;

        //Iterium collision
        private void OnTriggerEnter(Collider collision)
        {
            switch (collision.gameObject.tag)
            {
                //Collected by player
                case "Player":
                    CollectIterium.Invoke("player");
                    Destroy(gameObject);
                    break;

                //Collected by AI
                case "AI":
                    CollectIterium.Invoke("ai");
                    Destroy(gameObject);
                    break;
            }
        }
    }
}