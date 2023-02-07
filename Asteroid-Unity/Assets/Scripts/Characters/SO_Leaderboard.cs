using Iterium;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject containing a leaderboard list of high score items
/// </summary>

[CreateAssetMenu(fileName = "Leaderboard", menuName = "Add SO/Common/Leaderboard")]

public class SO_Leaderboard : ScriptableObject
{
  public List<LeaderboardItem> Leaderboard = new List<LeaderboardItem>();
}
