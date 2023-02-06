using UnityEngine;

//Collecting of Iterium by players or AI

public class Iterium : MonoBehaviour
{
    [Header("Collection Reward")]
    [SerializeField] private int score = 250;
    [SerializeField] private int xp = 25;
    [SerializeField] private int sfxIndex;


    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            //Collected by player
            case "Player":

                GameManager.Instance.player.Score += score;
                GameManager.Instance.player.XpCollected += xp;
                GameManager.Instance.player.IteriumCollected++;
                SoundManager.Instance.PlayEffect(sfxIndex);
                //Remove object
                Destroy(gameObject);
                break;

            //Bullet hits AI player
            case "AI":
                GameManager.Instance.aiPlayer.Score += score;
                GameManager.Instance.aiPlayer.XpCollected += xp;
                GameManager.Instance.aiPlayer.IteriumCollected++;
                //Remove object
                Destroy(gameObject);
                break;
        }
    }
}
