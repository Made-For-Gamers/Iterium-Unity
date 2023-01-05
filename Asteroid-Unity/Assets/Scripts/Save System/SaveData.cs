using UnityEngine;

/// <summary>
/// Class of all data fields that need to be save or loaded in the game
/// </summary>

[System.Serializable]

public class SaveData
{
    public int score;
    public int xp;

    //Constructor can be called to reset fields to defaults
    public SaveData()
    {
      
    }
}
