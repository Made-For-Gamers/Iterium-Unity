using UnityEngine;

/// <summary>
/// Class of all save data that need to be saved or loaded in the game
/// </summary>

[System.Serializable]

public class SaveData
{
    public int score;
    public int xp;
    public int shieldLvl;
    public int bulletLvl;
    public int iterium;
    public SO_Character character;

    //Constructor can be called to reset fields to defaults
    public SaveData()
    {
        //Reset game data to defaults
        score = 0;
        xp = 0;
        shieldLvl = 0;
        bulletLvl = 0;
        iterium = 0;
        character = null;
    }
}
