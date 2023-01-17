using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

//Collecting of Iterium by players or AI

public class Iterium : MonoBehaviour
{
    [Header("Collection Reward")]
    [SerializeField] private int score = 250;
    [SerializeField] private int xp = 25;

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Collected by player
            case "Player":

                GameManager.Instance.player.Score += score;
                GameManager.Instance.player.Xp += xp;
                GameManager.Instance.player.Iterium++;

                //Remove object
                Destroy(gameObject);
                break;

            //Bullet hits AI player
            case "AI":
                GameManager.Instance.aiPlayer.Score += score;
                GameManager.Instance.aiPlayer.Xp += xp;
                GameManager.Instance.aiPlayer.Iterium++;

                //Remove object
                Destroy(gameObject);
                break;
        }
    }
}
