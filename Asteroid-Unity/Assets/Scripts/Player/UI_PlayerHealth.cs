using UnityEngine;
using TMPro;

/// <summary>
/// Displays a players health - TextMeshPro text field
/// </summary>
public class UI_PlayerHealth : MonoBehaviour
{
    
    [SerializeField] private SO_Player player;
    private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        healthText.text = player.Health.ToString();
    }
}
