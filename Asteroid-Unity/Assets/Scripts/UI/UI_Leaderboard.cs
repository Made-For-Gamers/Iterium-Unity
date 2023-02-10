using UnityEngine.UIElements;
using UnityEngine;

namespace Iterium
{
    //Populate the leaderboard

    public class UI_Leaderboard : MonoBehaviour
    {
        [Header("Leaderboard Item UI Document")]
        [SerializeField] private VisualTreeAsset scoreRow;

        [Header("Leaderboard item fields")]
        [SerializeField] private string rank = "rank";
        [SerializeField] private string score = "score";
        [SerializeField] private string date = "date";
        [SerializeField] private string playerName = "playerName";

        private int scoreRank;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            GameManager.Instance.SortLeaderboard();

            //Populate leaderboard SrollView with rows
            foreach (LeaderboardItem item in GameManager.Instance.leaderboard.Leaderboard)
            {
                var rowTemplate = scoreRow.Instantiate();
                scoreRank++;
                rowTemplate.Q<Label>(rank).text = scoreRank.ToString();
                rowTemplate.Q<Label>(score).text = item.score.ToString();
                rowTemplate.Q<Label>(date).text = item.date.ToString();
                rowTemplate.Q<Label>(playerName).text = item.playerName;
                uiRoot.Q<ScrollView>().contentContainer.Add(rowTemplate);
            }
        }
    }
}