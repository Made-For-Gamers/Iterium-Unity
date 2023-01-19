using UnityEngine.UIElements;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private string playerScore = "playerScore";
    [SerializeField] private string playerIterium = "playerIterium";
    [SerializeField] private string playerMessage = "message";

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

        //Init player UI controls
        TextElement score = uiRoot.Q<TextElement>(playerScore);
        TextElement iterium = uiRoot.Q<TextElement>(playerIterium);
        TextElement message = uiRoot.Q<TextElement>(playerMessage);

        score.text = GameManager.Instance.player.Score.ToString();
        iterium.text = GameManager.Instance.player.IteriumCollected.ToString();
        message.text = "You did not beat any high scores.";
    }
}

