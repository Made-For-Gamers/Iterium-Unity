using TMPro;
using UnityEngine;

public class UI_XP : MonoBehaviour
{
    [SerializeField] private SO_Player player;
    private TextMeshProUGUI xpText;

    private void Start()
    {
        xpText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        xpText.text = player.Xp.ToString();
    }
}
