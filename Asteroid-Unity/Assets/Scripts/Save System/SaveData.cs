using UnityEngine;

/// <summary>
/// Class of all data fields that need to be save or loaded in the game
/// </summary>

[System.Serializable]

public class SaveData
{
    public int score;
    public int xp;
    public int shieldLvl;
    public int bulletLvl;
    public int iterium;
    public SO_Ship ship;

    //Constructor can be called to reset fields to defaults
    public SaveData()
    {
        //Reset game data to defaults
        score = 0;
        xp = 0;
        shieldLvl = 0;
        bulletLvl = 0;
        iterium = 0;
    }
}
