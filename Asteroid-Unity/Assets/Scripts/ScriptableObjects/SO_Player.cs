using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Player")]

// Player data
// Data is changed here during runtime and the game reads player data from here
// Save game data is also duplicated from here and you can therefore have a async save process when a cloud save handeler is used
// The default ScriptablObject values are updated after a save game load during startup;
// SO_Character is the currently selected faction character
// SO_Character contains a reference to the faction specific ship
// BulletLvl indicates which of the 3 bullets levels is fired from the SO_Bullet. (bullet level)
public class SO_Player : ScriptableObject
{
    [SerializeField] private string charName;
    [TextArea(5, 5)]
    [SerializeField] private string description;
    [SerializeField] private int xp;
    [SerializeField] private int score;
    [SerializeField] private SO_Character character;
    [SerializeField] private int health;
    [SerializeField] private int bulletLvl;
    [SerializeField] private int shieldLvl;
    [SerializeField] private int speedLvl;
    [SerializeField] private int iterium;
    [SerializeField] private int lives;

    //Events
    public UnityEvent onChange_Health;
    public UnityEvent onChange_Score;
    public UnityEvent onChange_Iterium;
    public UnityEvent onChange_Lives;

    public string CharName { get => charName; set => charName = value; }
    public string Description { get => description; set => description = value; }
    public int Xp
    {
        get => xp;
        set
        {
            xp = value;
            GameManager.Instance.saveData.xp = value;
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            GameManager.Instance.saveData.score = value;
            onChange_Score.Invoke();
        }
    }
    public SO_Character Character
    {
        get => character;
        set
        {
            character = value;
            GameManager.Instance.saveData.character = value;
        }
    }
    public int Health
    {
        get => health;
        set
        {
            health = value;
            onChange_Health.Invoke();
        }
    }
    public int BulletLvl
    {
        get => bulletLvl;
        set
        {
            bulletLvl = value;
            GameManager.Instance.saveData.bulletLvl = value;
        }
    }
    public int ShieldLvl
    {
        get => shieldLvl;
        set
        {
            shieldLvl = value;
            GameManager.Instance.saveData.shieldLvl = value;
        }
    }
    public int Iterium
    {
        get => iterium;
        set
        {
            iterium = value;
            GameManager.Instance.saveData.iterium = value;
            onChange_Iterium.Invoke();
        }
    }

    public int SpeedLvl
    {
        get => speedLvl;
        set
        {
            speedLvl = value;
            GameManager.Instance.saveData.speedLvl = value;
        }
    }

    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
            onChange_Lives.Invoke();
        }
    }
}
