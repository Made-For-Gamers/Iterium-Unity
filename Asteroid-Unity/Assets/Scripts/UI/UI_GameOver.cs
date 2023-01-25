using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Over UI
/// * Shows score and iterium collected
/// *Indicates if a high score is achieved
/// *Display specific message depending on game performance
/// </summary>
public class UI_GameOver : MonoBehaviour
{
    [Header("Game scene")]
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

        arenaScore = GameManager.Instance.player.Score;
        score.text = arenaScore.ToString();
        iterium.text = GameManager.Instance.player.IteriumCollected.ToString();
        GameManager.Instance.player.Iterium += GameManager.Instance.player.IteriumCollected;
        GameManager.Instance.SortLeaderboard();       
        if (GameManager.Instance.leaderboard[GameManager.Instance.leaderboard.Count-1].score <= arenaScore)
        { 
            message.text = "Congratulations a new high score!";
            GameManager.Instance.AddLeaderboardItem();
            SoundManager.Instance.PlayMusic(2, false, true);
        }
        else
        {
            SoundManager.Instance.PlayMusic(1, false, true);
            switch (arenaScore)
            {
                case >= 200000:
                    message.text = "Super score!, an arcade pro.";
                    break;
                case >= 100000:
                    message.text = "Gosh brilliant score!, impressed";
                    break;
                case >= 50000:
                    message.text = "Well done!, a good score.";
                    break;
                case < 50000:
                    message.text = "Descent score, good game.";
                    break;
            }
        }      
        GameManager.Instance.SaveGame();
        rematch.clicked += Rematch;
    }

    private void Rematch()
    {
        SceneManager.LoadScene(gameScene);
    }
}

