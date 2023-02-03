using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject containt a leaderboard list of high scores
/// </summary>

[CreateAssetMenu(fileName = "Leaderboard", menuName = "Add SO/Common/Leaderboard")]

public class SO_Leaderboard : ScriptableObject
{
  public List<LeaderboardItem> Leaderboard = new List<LeaderboardItem>();
}
