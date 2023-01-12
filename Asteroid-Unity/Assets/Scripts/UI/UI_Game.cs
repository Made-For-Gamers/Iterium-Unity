using UnityEngine.UIElements;
using UnityEngine;

/// <summary>
/// Game scene UI data (UI Toolkit)
/// Player UI fields: score, health, iterium crystals, lives
/// </summary>
public class UI_Game : MonoBehaviour
{
    [Header("UI Toolkit Fields")]
    [SerializeField] private string playerScore;
    [SerializeField] private string playerHealth;
    [SerializeField] private string playerIterium;
    [SerializeField] private string aiScore;
    [SerializeField] private string aiHealth;
    [SerializeField] private string aiIterium;


    private TextElement playerTextScore;
    private ProgressBar playerBarHealth;
    private TextElement playerTextIterium;
    private TextElement aiTextScore;
    private ProgressBar aiBarHealth;
    private TextElement aiTextIterium;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

        //Player UI fields
        playerTextScore = uiRoot.Q<TextElement>(playerScore);
        playerBarHealth = uiRoot.Q<ProgressBar>(playerHealth);
        playerTextIterium = uiRoot.Q<TextElement>(playerIterium);

        //AI UI fields
        aiTextScore = uiRoot.Q<TextElement>(aiScore);
        aiBarHealth = uiRoot.Q<ProgressBar>(aiHealth);
        aiTextIterium = uiRoot.Q<TextElement>(aiIterium);
    }

    private void Start()
    {
        //Set UI values on start
        ChangeScore();
        ChangeHealth(); 
        ChangeIterium();

        //UI update event listeners
        GameManager.Instance.player.onChange_Health.AddListener(ChangeHealth);
        GameManager.Instance.player.onChange_Score.AddListener(ChangeScore);
        GameManager.Instance.player.onChange_Iterium.AddListener(ChangeIterium);
        GameManager.Instance.aiPlayer.onChange_Health.AddListener(ChangeAiHealth);
        GameManager.Instance.aiPlayer.onChange_Score.AddListener(ChangeAiScore);
        GameManager.Instance.aiPlayer.onChange_Iterium.AddListener(ChangeAiIterium);
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
}
