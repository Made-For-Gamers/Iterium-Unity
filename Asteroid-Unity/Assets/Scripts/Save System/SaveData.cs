using UnityEngine;

// Class of all data that needs to be saved/loaded

[System.Serializable]

public class SaveData
{   
    public string profileName;
    public string bio;
    public string email;
    public int xp;
    public int level;
    public int shieldLvlUs;
    public int bulletLvlUs;
    public int speedLvlUs;
    public int shieldLvlUssr;
    public int bulletLvlUssr;
    public int speedLvlUssr;
    public int shieldLvlChn;
    public int bulletLvlChn;
    public int speedLvlChn;
    public int iterium;
    public SO_Faction character;
    public float effectVolume;
    public float musicVolume;
}
