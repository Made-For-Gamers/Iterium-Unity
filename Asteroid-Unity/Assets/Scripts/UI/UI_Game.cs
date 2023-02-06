using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Net.Http.Headers;

/// <summary>
/// Game scene UI data (UI Toolkit)
/// Player UI fields: score, health, iterium crystals, lives
/// </summary>
public class UI_Game : MonoBehaviour
{
    [Header("Drag scene to load")]
#if UNITY_EDITOR
    public UnityEditor.SceneAsset destinationScene;
    private void OnValidate()
    {
        if (destinationScene != null)
        {
            sceneName = destinationScene.name;
        }
    }
#endif
    [HideInInspector] public string sceneName;

    [Header("Player UI Elements")]
    [SerializeField] private string playerName = "player1";
    [SerializeField] private string playerScore = "scorePlayer1";
    [SerializeField] private string playerIterium = "iteriumPlayer1";

    [Header("AI Player UI Elements")]
    [SerializeField] private string aiName = "player2";
    [SerializeField] private string aiScore = "scorePlayer2";
    [SerializeField] private string aiIterium = "iteriumPlayer2";

    [Header("Lives UI Elements")]
    [SerializeField] private string ship1Player1 = "ship1Player1";
    [SerializeField] private string ship2Player1 = "ship2Player1";
    [SerializeField] private string ship3Player1 = "ship3Player1";
    [SerializeField] private string ship4Player1 = "ship4Player1";
    [SerializeField] private string ship1Player2 = "ship1Player2";
    [SerializeField] private string ship2Player2 = "ship2Player2";
    [SerializeField] private string ship3Player2 = "ship3Player2";

    //Slider planes (shader graph materials)
    [Header("Player Health Sliders")]
    [SerializeField] private GameObject sliderPlayerHealth;
    [SerializeField] private GameObject sliderAiHealth;

    [Header("Pause Element")]
    [SerializeField] private string pause = "pause";
    [SerializeField] private string exitGame = "exitGame";
    [SerializeField] private string continueGame = "continueGame";

    private bool extraLife;

    //Player UI controls
    private TextElement playerTextName;
    private TextElement playerTextScore;
    private TextElement playerTextIterium;
    private VisualElement player1Ship1;
    private VisualElement player1Ship2;
    private VisualElement player1Ship3;
    private VisualElement player1Ship4;

    //AI Player UI controls   
    private TextElement aiTextName;
    private TextElement aiTextScore;
    private TextElement aiTextIterium;
    private VisualElement player2Ship1;
    private VisualElement player2Ship2;
    private VisualElement player2Ship3;
    private Renderer rendererPlayer;
    private Renderer rendererAi;
    private Material matPlayerHealth;
    private Material matAiHealth;

    private VisualElement pausePanel;
    private Button exitButton;
    private Button continueButton;
    private InputSystem input;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

        //Init player UI
        playerTextName = uiRoot.Q<TextElement>(playerName);
        playerTextScore = uiRoot.Q<TextElement>(playerScore);
        playerTextIterium = uiRoot.Q<TextElement>(playerIterium);
        player1Ship1 = uiRoot.Q<VisualElement>(ship1Player1);
        player1Ship2 = uiRoot.Q<VisualElement>(ship2Player1);
        player1Ship3 = uiRoot.Q<VisualElement>(ship3Player1);
        player1Ship4 = uiRoot.Q<VisualElement>(ship4Player1);
        player1Ship4.style.unityBackgroundImageTintColor = Color.black;

        //Init AI UI
        aiTextName = uiRoot.Q<TextElement>(aiName);
        aiTextScore = uiRoot.Q<TextElement>(aiScore);
        aiTextIterium = uiRoot.Q<TextElement>(aiIterium);
        player2Ship1 = uiRoot.Q<VisualElement>(ship1Player2);
        player2Ship2 = uiRoot.Q<VisualElement>(ship2Player2);
        player2Ship3 = uiRoot.Q<VisualElement>(ship3Player2);

        //Init Pause UI
        pausePanel = uiRoot.Q<VisualElement>(pause);
        exitButton = uiRoot.Q<Button>(exitGame);
        continueButton = uiRoot.Q<Button>(continueGame);

        //Events
        exitButton.clicked += ExitGame;
        continueButton.clicked += ExitPause;

        //Init Input System
        input = new InputSystem();
        input.Player.Enable();
        input.Player.Pause.started += Pause;
    }

    private void OnDisable()
    {
        input.Player.Pause.started -= Pause;
    }

    private void Start()
    {
        //Init Health Sliders
        rendererPlayer = sliderPlayerHealth.GetComponent<Renderer>();
        rendererAi = sliderAiHealth.GetComponent<Renderer>();
        matPlayerHealth = rendererPlayer.material;
        matAiHealth = rendererAi.material;

        //Set UI values on start
        GameManager.Instance.ResetArena();
        ChangeNames();
        ChangeScore();
        ChangeAiScore();
        ChangeHealth();
        ChangeAiHealth();
        ChangeIterium();
        ChangeAiIterium();

        //Player update event listeners
        GameManager.Instance.player.onChange_Health.AddListener(ChangeHealth);
        GameManager.Instance.player.onChange_Score.AddListener(ChangeScore);
        GameManager.Instance.player.onChange_IteriumCollected.AddListener(ChangeIterium);
        GameManager.Instance.player.onChange_Lives.AddListener(ChangeLives);

        //AI Player update event listeners
        GameManager.Instance.aiPlayer.onChange_Health.AddListener(ChangeAiHealth);
        GameManager.Instance.aiPlayer.onChange_Score.AddListener(ChangeAiScore);
        GameManager.Instance.aiPlayer.onChange_IteriumCollected.AddListener(ChangeAiIterium);
        GameManager.Instance.aiPlayer.onChange_Lives.AddListener(ChangeAiLives);

        if (!GameManager.Instance.aiPermadeath)
        {
            player2Ship1.style.unityBackgroundImageTintColor = Color.black;
            player2Ship2.style.unityBackgroundImageTintColor = Color.black;
            player2Ship3.style.unityBackgroundImageTintColor = Color.black;
        }
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausePanel.style.display = DisplayStyle.None;
        }
        else
        {
            pausePanel.style.display = DisplayStyle.Flex;
            Time.timeScale = 0;
        }
    }

    private void ExitGame()
    {
        PauseGame();
        SceneManager.LoadScene(sceneName);
    }

    private void ExitPause()
    {
        GameManager.Instance.ResetArena();
        PauseGame();
    }

    private void ChangeNames()
    {
        playerTextName.text = GameManager.Instance.player.ProfileName.ToString();
        aiTextName.text = GameManager.Instance.aiPlayer.ProfileName.ToString();
    }

    //Player UI updates
    private void ChangeScore()
    {
        playerTextScore.text = GameManager.Instance.player.Score.ToString();
        if (GameManager.Instance.player.Score >= 100000 && extraLife == false)
        {
            switch (GameManager.Instance.player.Lives)
            {
                case 1:
                    player1Ship2.style.unityBackgroundImageTintColor = Color.white;
                    break;
                case 2:
                    player1Ship3.style.unityBackgroundImageTintColor = Color.white;
                    break;
                case 3:
                    player1Ship4.style.unityBackgroundImageTintColor = Color.white;
                    break;
            }
            GameManager.Instance.player.Lives++;
            extraLife = true;
        }
    }

    private void ChangeHealth()
    {
        matPlayerHealth.SetFloat("_RemovedSeg", 10 - (GameManager.Instance.player.Health / 10));
    }

    private void ChangeIterium()
    {
        playerTextIterium.text = GameManager.Instance.player.IteriumCollected.ToString();
    }

    //AI UI updates
    private void ChangeAiScore()
    {
        aiTextScore.text = GameManager.Instance.aiPlayer.Score.ToString();
    }

    private void ChangeAiHealth()
    {
        matAiHealth.SetFloat("_RemovedSeg", 10 - (GameManager.Instance.aiPlayer.Health / 10));
    }

    private void ChangeAiIterium()
    {
        aiTextIterium.text = GameManager.Instance.aiPlayer.IteriumCollected.ToString();
    }

    private void ChangeLives()
    {
        switch (GameManager.Instance.player.Lives)
        {
            case 1:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship2.style.unityBackgroundImageTintColor = Color.red;
                player1Ship3.style.unityBackgroundImageTintColor = Color.red;
                player1Ship4.style.unityBackgroundImageTintColor = Color.red;
                break;
            case 2:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship3.style.unityBackgroundImageTintColor = Color.red;
                player1Ship4.style.unityBackgroundImageTintColor = Color.red;
                break;
            case 3:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship2.style.unityBackgroundImageTintColor = Color.white;
                player1Ship3.style.unityBackgroundImageTintColor = Color.white;
                player1Ship4.style.unityBackgroundImageTintColor = Color.red;
                break;
            case 4:
                player1Ship1.style.unityBackgroundImageTintColor = Color.white;
                player1Ship2.style.unityBackgroundImageTintColor = Color.white;
                player1Ship3.style.unityBackgroundImageTintColor = Color.white;
                player1Ship4.style.unityBackgroundImageTintColor = Color.white;
                break;
        }
    }

    private void ChangeAiLives()
    {
        switch (GameManager.Instance.aiPlayer.Lives)
        {
            case 1:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship2.style.unityBackgroundImageTintColor = Color.red;
                player2Ship3.style.unityBackgroundImageTintColor = Color.red;
                break;
            case 2:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship3.style.unityBackgroundImageTintColor = Color.red;
                break;
            case 3:
                player2Ship1.style.unityBackgroundImageTintColor = Color.white;
                player2Ship2.style.unityBackgroundImageTintColor = Color.white;
                player2Ship3.style.unityBackgroundImageTintColor = Color.white;
                break;
        }
    }
}
