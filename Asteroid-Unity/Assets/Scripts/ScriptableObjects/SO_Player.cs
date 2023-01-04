using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Player")]

//Default properties and data of a player
public class SO_Player : ScriptableObject, ISave
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private int xp;
    [SerializeField] private int score;
    [SerializeField] private SO_Ship ship;
    [SerializeField] private int health;
    [SerializeField] private int bulletLvl;
    [SerializeField] private int shieldLvl;

    public string CharName { get => charName; set => charName = value; } 
    public string Description { get => description; set => description = value; } 
    public int Xp { get => xp; set => xp = value; }
    public int Score { get => score; set => score = value; }
    public SO_Ship Ship { get => ship; set => ship = value; }
    public int Health { get => health; set => health = value; }
    public int BulletLvl { get => bulletLvl; set => bulletLvl = value; }
    public int ShieldLvl { get => shieldLvl; set => shieldLvl = value; }

    public void LoadData(SaveData saveData)
    {
        score = saveData.score;
        xp = saveData.xp;
    }

    public void SaveData(ref SaveData saveData)
    {
       saveData.score = score;
       saveData.xp = xp;
    }
}
