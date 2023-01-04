using UnityEngine;

[System.Serializable]

//All data to be saved/loaded
public class SaveData
{
    public int score;
    public int xp;

    public SaveData()
    {
        score = 0;
        xp = 0;
    }
}
