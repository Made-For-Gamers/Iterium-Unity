using UnityEngine.UIElements;
using UnityEngine;

/// <summary>
/// Game scene UI data (UI Toolkit)
/// Player UI fields: score, health, iterium crystals, lives
/// </summary>
public class UI_Game : MonoBehaviour
{
    [Header("Player UI Elements")]
    [SerializeField] private string playerScore = "scorePlayer1";
    [SerializeField] private string playerHealth = "healthPlayer1";
    [SerializeField] private string playerIterium = "iteriumPlayer1";

    [Header("AI Player UI Elements")]
    [SerializeField] private string aiScore = "scorePlayer2";
    [SerializeField] private string aiHealth = "healthPlayer2";
    [SerializeField] private string aiIterium = "iteriumPlayer2";

    [Header("Ship Lives UI Elements")]
    [SerializeField] private string ship1Player1 = "ship1Player1";
    [SerializeField] private string ship2Player1 = "ship2Player1";
    [SerializeField] private string ship3Player1 = "ship3Player1";
    [SerializeField] private string ship1Player2 = "ship1Player2";
    [SerializeField] private string ship2Player2 = "ship2Player2";
    [SerializeField] private string ship3Player2 = "ship3Player2";


    //Player UI controls
    private TextElement playerTextScore;
    private ProgressBar playerBarHealth;
    private TextElement playerTextIterium;
    private VisualElement player1Ship1;
    private VisualElement player1Ship2;
    private VisualElement player1Ship3;

    //AI Player UI controls
    private TextElement aiTextScore;
    private ProgressBar aiBarHealth;
    private TextElement aiTextIterium;
    private VisualElement player2Ship1;
    private VisualElement player2Ship2;
    private VisualElement player2Ship3;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

        //Init player UI controls
        playerTextScore = uiRoot.Q<TextElement>(playerScore);
        playerBarHealth = uiRoot.Q<ProgressBar>(playerHealth);
        playerTextIterium = uiRoot.Q<TextElement>(playerIterium);
        player1Ship1 = uiRoot.Q<VisualElement>(ship1Player1);
        player1Ship2 = uiRoot.Q<VisualElement>(ship2Player1);
        player1Ship3 = uiRoot.Q<VisualElement>(ship3Player1);

        //Init AI UI controls
        aiTextScore = uiRoot.Q<TextElement>(aiScore);
        aiBarHealth = uiRoot.Q<ProgressBar>(aiHealth);
        aiTextIterium = uiRoot.Q<TextElement>(aiIterium);
        player2Ship1 = uiRoot.Q<VisualElement>(ship1Player2);
        player2Ship2 = uiRoot.Q<VisualElement>(ship2Player2);
        player2Ship3 = uiRoot.Q<VisualElement>(ship3Player2);
    }

    private void Start()
    {
        //Set UI values on start
        ChangeScore();
        ChangeHealth(); 
        ChangeIterium();

        //Player update event listeners
        GameManager.Instance.player.onChange_Health.AddListener(ChangeHealth);
        GameManager.Instance.player.onChange_Score.AddListener(ChangeScore);
        GameManager.Instance.player.onChange_Iterium.AddListener(ChangeIterium);
        GameManager.Instance.player.onChange_Lives.AddListener(ChangePlayerLives);

        //AI Player update event listeners
        GameManager.Instance.aiPlayer.onChange_Health.AddListener(ChangeAiHealth);
        GameManager.Instance.aiPlayer.onChange_Score.AddListener(ChangeAiScore);
        GameManager.Instance.aiPlayer.onChange_Iterium.AddListener(ChangeAiIterium);
        GameManager.Instance.aiPlayer.onChange_Lives.AddListener(ChangeAiLives);
    } 
    
    //Player UI updates
    private void ChangeScore()
    {
        playerTextScore.text = GameManager.Instance.player.Score.ToString();
    }

    private void ChangeHealth()
    {
        playerBarHealth.value = GameManager.Instance.player.Health;
    }  

    private void ChangeIterium()
    {
        playerTextIterium.text = GameManager.Instance.player.Iterium.ToString();
    }

    //AI UI updates
    private void ChangeAiScore()
    {
        aiTextScore.text = GameManager.Instance.aiPlayer.Score.ToString();
    }

    private void ChangeAiHealth()
    {
        aiBarHealth.value = GameManager.Instance.aiPlayer.Health;
    }
   
    private void ChangeAiIterium()
    {
        aiTextIterium.text = GameManager.Instance.aiPlayer.Iterium.ToString();
    }

    private void ChangePlayerLives()
    {
        switch (GameManager.Instance.player.Lives)
        {
            case 1:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship2.style.unityBackgroundImageTintColor = Color.black;
                player1Ship3.style.unityBackgroundImageTintColor = Color.black;
                break;
            case 2:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship3.style.unityBackgroundImageTintColor = Color.black;
                break;
            case 3:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship2.style.unityBackgroundImageTintColor = Color.white;
                player1Ship3.style.unityBackgroundImageTintColor = Color.white;
                break;
        }
    }

    private void ChangeAiLives()
    {
        switch (GameManager.Instance.aiPlayer.Lives)
        {
            case 1:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship2.style.unityBackgroundImageTintColor = Color.black;
                player2Ship3.style.unityBackgroundImageTintColor = Color.black;
                break;
            case 2:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship3.style.unityBackgroundImageTintColor = Color.black;
                break;
            case 3:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship2.style.unityBackgroundImageTintColor = Color.white;
                player2Ship3.style.unityBackgroundImageTintColor = Color.white;
                break;
        }
    }
}
