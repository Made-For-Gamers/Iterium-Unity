using TMPro;
using UnityEngine;

/// <summary>
/// Displays a players score - TextMeshPro text field
/// </summary>
public class UI_Score : MonoBehaviour
{
    [SerializeField] private SO_Player player;
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scoreText.text = player.Score.ToString();
    }
}
