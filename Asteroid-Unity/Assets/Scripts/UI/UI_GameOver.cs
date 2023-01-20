using UnityEngine.UIElements;
using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// Game Over UI
/// * Shows score and iterium collected
/// *Indicates if a high score is achieved
/// *Display specific message depending on game performance
/// </summary>
public class UI_GameOver : MonoBehaviour
{
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
        rematch.clicked += Rematch;
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

    private void Rematch()
    {
        GameManager.Instance.NewArena();
    }
}

