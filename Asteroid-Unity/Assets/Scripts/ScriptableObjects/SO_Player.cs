using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Add SO/Objects/Player")]

// Player data
// The default ScriptablObject values are updated after a game load during startup;

public class SO_Player : ScriptableObject
{
    [SerializeField] private string profileName = "Player 1";
    [TextArea(5, 5)]
    [SerializeField] private string bio;
    [SerializeField] private string email;
    [SerializeField] private int xp;
    [SerializeField] private int xpCollected;
    [SerializeField] private int level = 1;
    [SerializeField] private int score;
    [SerializeField] private SO_Faction character;
    [SerializeField] private int health;
    [SerializeField] private int iterium;
    [SerializeField] private int iteriumCollected;
    [SerializeField] private int lives = 3;
    [SerializeField] private int bulletLvl = 1;
    [SerializeField] private int shieldLvl = 1;
    [SerializeField] private int speedLvl = 1;
    [SerializeField] private int bulletLvlUs = 1;
    [SerializeField] private int shieldLvlUs = 1;
    [SerializeField] private int speedLvlUs = 1;
    [SerializeField] private int bulletLvlUssr = 1;
    [SerializeField] private int shieldLvlUssr = 1;
    [SerializeField] private int speedLvlUssr = 1;
    [SerializeField] private int bulletLvlChn = 1;
    [SerializeField] private int shieldLvlChn = 1;
    [SerializeField] private int speedLvlChn = 1;
    [SerializeField] private float effectsVolume;
    [SerializeField] private float musicVolume;

    //Events
    public UnityEvent onChange_Health;
    public UnityEvent onChange_Score;
    public UnityEvent onChange_Iterium;
    public UnityEvent onChange_IteriumCollected;
    public UnityEvent onChange_Lives;
    public UnityEvent onChange_bulletLvl;
    public UnityEvent onChange_shieldLvl;
    public UnityEvent onChange_speedLvl;


    public string ProfileName
    {
        get => profileName;
        set
        {
            profileName = value;
        }
    }
    public string Bio { get => bio; set => bio = value; }
    public string Email { get => email; set => email = value; }
    public int Xp
    {
        get => xp;
        set
        {
            xp = value;
        }
    }
    public int XpCollected
    {
        get => xpCollected;
        set
        {
            xpCollected = value;
        }
    }
    public int Level
    {
        get => level;
        set
        {
            level = value;
        }
    }

    public int Score
    {
        get => score;
        set
        {
            score = value;
            onChange_Score.Invoke();
        }
    }
    public SO_Faction Character
    {
        get => character;
        set
        {
            character = value;
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

    public int Iterium
    {
        get => iterium;
        set
        {
            iterium = value;
            onChange_Iterium.Invoke();
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

    public int IteriumCollected
    {
        get => iteriumCollected;
        set
        {
            iteriumCollected = value;
            onChange_IteriumCollected.Invoke();
        }
    }

    public int BulletLvl
    {
        get => bulletLvl;
        set
        {
            bulletLvl = value;
            onChange_bulletLvl.Invoke();
        }
    }
    public int ShieldLvl
    {
        get => shieldLvl;
        set
        {
            shieldLvl = value;
            onChange_shieldLvl.Invoke();
        }
    }
    public int SpeedLvl
    {
        get => speedLvl;
        set
        {
            speedLvl = value;
            onChange_speedLvl.Invoke();
        }
    }
    public int BulletLvlUs { get => bulletLvlUs; set => bulletLvlUs = value; }
    public int ShieldLvlUs { get => shieldLvlUs; set => shieldLvlUs = value; }
    public int SpeedLvlUs { get => speedLvlUs; set => speedLvlUs = value; }
    public int BulletLvlUssr { get => bulletLvlUssr; set => bulletLvlUssr = value; }
    public int ShieldLvlUssr { get => shieldLvlUssr; set => shieldLvlUssr = value; }
    public int SpeedLvlUssr { get => speedLvlUssr; set => speedLvlUssr = value; }
    public int BulletLvlChn { get => bulletLvlChn; set => bulletLvlChn = value; }
    public int ShieldLvlChn { get => shieldLvlChn; set => shieldLvlChn = value; }
    public int SpeedLvlChn { get => speedLvlChn; set => speedLvlChn = value; }
    public float EffectsVolume { get => effectsVolume; set => effectsVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
}
