using UnityEngine;

/// <summary>
/// Class of all save data that need to be saved or loaded in the game
/// </summary>

[System.Serializable]

public class SaveData
{   
    public string profileName;
    public int xp;
    public int shieldLvl;
    public int bulletLvl;
    public int speedLvl;
    public int iterium;
    public SO_Character character;
    public SO_Leaderboard leaderboard;
        
}
