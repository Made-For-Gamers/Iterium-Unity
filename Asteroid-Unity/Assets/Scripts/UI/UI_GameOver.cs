using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Over UI
/// * Shows score and iterium collected
/// *Indicates if a high score is achieved
/// *Calculate XP/Leveling
/// *Display specific message depending on game performance
/// </summary>
public class UI_GameOver : MonoBehaviour
{
    [Header("Game Play Scene")]
#if UNITY_EDITOR
    public UnityEditor.SceneAsset destinationScene;
    private void OnValidate()
    {
        if (destinationScene != null)
        {
            gameScene = destinationScene.name;
        }
    }
#endif
    [HideInInspector] public string gameScene;

    [Header("UI Elements")]
    [SerializeField] private string playerScore = "playerScore";
    [SerializeField] private string playerIterium = "playerIterium";
    [SerializeField] private string playerMessage = "message";
    [SerializeField] private string rematchButton = "rematch";

    private int arenaScore;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

        //Init player UI controls
        TextElement score = uiRoot.Q<TextElement>(playerScore);
        TextElement iterium = uiRoot.Q<TextElement>(playerIterium);
        TextElement message = uiRoot.Q<TextElement>(playerMessage);
        Button rematch = uiRoot.Q<Button>(rematchButton);

        //Events
        rematch.clicked += Rematch;

        //Score
        arenaScore = GameManager.Instance.player.Score;
        score.text = arenaScore.ToString();

        //Iterium
        iterium.text = GameManager.Instance.player.IteriumCollected.ToString();
        GameManager.Instance.CalculateIterium();

        //XP
        GameManager.Instance.CalculateXP();

        //AI firepower upgrades
        if (GameManager.Instance.aiPlayer.BulletLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.firepowerLevel1)
        {
            //Upgrade bullet to lvl 2
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.firepowerLevel1;
            GameManager.Instance.aiPlayer.BulletLvl = 2;
        }
        else if (GameManager.Instance.aiPlayer.BulletLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.firepowerLevel2)
        {
            //Upgrade bullet to lvl 3
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.firepowerLevel2;
            GameManager.Instance.aiPlayer.BulletLvl = 3;
        }

        //AI shield upgrades
        if (GameManager.Instance.aiPlayer.ShieldLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.shieldLevel1)
        {
            //Upgrade shield to lvl 2
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.shieldLevel1;
            GameManager.Instance.aiPlayer.ShieldLvl = 2;
        }
        else if (GameManager.Instance.aiPlayer.ShieldLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.shieldLevel2)
        {
            //Upgrade shield to lvl 3
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.shieldLevel2;
            GameManager.Instance.aiPlayer.ShieldLvl = 3;
        }

        if (GameManager.Instance.aiPlayer.SpeedLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.speedLevel1)
        {
            //Upgrade speed to lvl 2
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.speedLevel1;
            GameManager.Instance.aiPlayer.SpeedLvl = 2;
        }
        else if (GameManager.Instance.aiPlayer.SpeedLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.speedLevel2)
        {
            //Upgrade speed to lvl 3
            GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.speedLevel2;
            GameManager.Instance.aiPlayer.SpeedLvl = 3;
        }

        //Player leaderboard challenge
        if (GameManager.Instance.leaderboard[GameManager.Instance.leaderboard.Count - 1].score <= arenaScore)
        {
            //High Score greeting
            message.text = "Congratulations " + GameManager.Instance.player.ProfileName + "!, a new high score.";
            GameManager.Instance.AddLeaderboardItem(true);
            SoundManager.Instance.PlayMusic(2, false, true);
        }
        else
        {
            //Normal greeting
            SoundManager.Instance.PlayMusic(1, false, true);
            switch (arenaScore)
            {
                case >= 200000:
                    message.text = "Super score " + GameManager.Instance.player.ProfileName + "!, an arcade pro.";
                    break;
                case >= 100000:
                    message.text = "Gosh brilliant score " + GameManager.Instance.player.ProfileName + "!, impressed";
                    break;
                case >= 50000:
                    message.text = "Well done " + GameManager.Instance.player.ProfileName + "!, a good score.";
                    break;
                case < 50000:
                    message.text = "Descent score " + GameManager.Instance.player.ProfileName + "!, good game.";
                    break;
            }
        }

        //AI leaderboard challenge
        arenaScore = GameManager.Instance.aiPlayer.Score;
        if (GameManager.Instance.leaderboard[GameManager.Instance.leaderboard.Count - 1].score <= arenaScore)
        {
            GameManager.Instance.AddLeaderboardItem(false);
        }

        //Save Game
        GameManager.Instance.SaveGame();
    }

    private void Rematch()
    {
        GameManager.Instance.NewArena();
        SceneManager.LoadScene(gameScene);
    }
}

