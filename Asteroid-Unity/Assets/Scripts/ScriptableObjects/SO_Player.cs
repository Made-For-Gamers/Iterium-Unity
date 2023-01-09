using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Player")]

// Player data
// Data is changed here during runtime with save game data (which can happen in a async proicess and not effect the game).
// The default ScriptablObject values are updated after a game load at startup;
public class SO_Player : ScriptableObject
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
    [SerializeField] private int iterium;

    public string CharName { get => charName; set => charName = value; }
    public string Description { get => description; set => description = value; }
    public int Xp
    {
        get => xp;
        set
        {
            xp = value;
            Singleton.Instance.saveData.xp = value;
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            Singleton.Instance.saveData.score = value;
        }
    }
    public SO_Ship Ship
    {
        get => ship;
        set
        {
            ship = value;
            Singleton.Instance.saveData.ship = value;
        }
    }
    public int Health { get => health; set => health = value; }
    public int BulletLvl
    {
        get => bulletLvl;
        set
        {
            bulletLvl = value;
            Singleton.Instance.saveData.bulletLvl = value;
        }
    }
    public int ShieldLvl
    {
        get => shieldLvl;
        set
        {
            shieldLvl = value;
            Singleton.Instance.saveData.shieldLvl = value;
        }
    }
    public int Iterium
    {
        get => iterium;
        set
        {
            iterium = value;
            Singleton.Instance.saveData.iterium = value;
        }
    }
}
