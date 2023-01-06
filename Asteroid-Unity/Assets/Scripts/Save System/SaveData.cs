using UnityEngine;

/// <summary>
/// Class of all data fields that need to be save or loaded in the game
/// </summary>

[System.Serializable]

public class SaveData
{
    public int score;
    public int xp;
    public int shield;
    public SO_Player player;
    public SO_Ship ship;
    public SO_Bullet bullet;

    //Constructor can be called to reset fields to defaults
    public SaveData()
    {
        score = 0;
        xp = 0;
    }
}
