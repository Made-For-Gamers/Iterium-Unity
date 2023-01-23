using UnityEngine.UIElements;
using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [Header("Leaderboard Item - UI Document")]
    [SerializeField] private VisualTreeAsset scoreRow;

    [Header("Leaderboard item - UI Elements")]
    [SerializeField] private string rank = "rank";
    [SerializeField] private string score = "score";
    [SerializeField] private string date = "date";
    [SerializeField] private string playerName = "playerName";

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        GameManager.Instance.SortLeaderboard();
        foreach (LeaderboardItem item in GameManager.Instance.leaderboard)
        {
            var rowTemplate = scoreRow.Instantiate();
            rowTemplate.Q<Label>(rank).text = "1";
            rowTemplate.Q<Label>(score).text = item.score.ToString();
            rowTemplate.Q<Label>(date).text = item.date.ToString();
            rowTemplate.Q<Label>(playerName).text = item.playerName;
            uiRoot.Q<ScrollView>().contentContainer.Add(rowTemplate);
        }
    }
}
