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

    private TextElement textScore;
    private ProgressBar ProgressbarHealth;
    private TextElement textIterium;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        textScore = uiRoot.Q<TextElement>(playerScore);
        ProgressbarHealth = uiRoot.Q<ProgressBar>(playerHealth);
        textIterium = uiRoot.Q<TextElement>(playerIterium);
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
    } 
    
    private void ChangeScore()
    {
        textScore.text = GameManager.Instance.player.Score.ToString();
    }

    private void ChangeHealth()
    {
        ProgressbarHealth.value = GameManager.Instance.player.Health;
    }  

    private void ChangeIterium()
    {
        textIterium.text = GameManager.Instance.player.Iterium.ToString();
    }
}
