using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton manager to manage main game specific data
/// </summary>

public class GameManager : Singleton<GameManager>
{
    [Header("Save Game")]
    [SerializeField] private string saveFile;
    [SerializeField] private string saveFileLeaderboard;
    [SerializeField] private int leaderboardSize = 50;

    [Header("Player Settings")]
    public SO_Player player;
    public SO_Factions factions;
    public float deathRespawnTime = 4f;
    public int xpLevelSteps = 1000;
    public int maxLevel = 50;

    [Header("AI Settings")]
    public SO_Player aiPlayer;
    public bool aiPermadeath;
    [HideInInspector] public GameObject aiTarget;

    [Header("NPC Settings")]
    public SO_Player npcPlayer;

    [Header("Iterium Crystals")]
    public SO_GameObjects iterium;
    public int iteriumChance = 20;

    [Space(10)]
    //Save data objects
    public List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();
    [HideInInspector] public SaveData saveData = new SaveData();

    //Spawn points
    [HideInInspector] public Transform playerSpawner;
    [HideInInspector] public Transform aiSpawner;

    private FileSaveHandler fileSaveHandler;

    private void Start()
    {
        this.fileSaveHandler = new FileSaveHandler(Application.persistentDataPath);
        LoadGame();
        //ResetGame();
    }

    //Find either Player or NPC for AI to target
    public void FindAiTarget(bool npc)
    {
        if (npc)
        {
            aiTarget = GameObject.Find("AI");
        }
        else
        {
            aiTarget = GameObject.Find("Player");
        }
    }

    private void SelectAiPlayer()
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        aiPlayer.Character = factions.Factions[rnd];
    }

    public void SaveGame()
    {
        //Update data to be saved
        saveData.profileName = player.ProfileName;
        saveData.bio = player.Bio;
        saveData.email = player.Email;
        saveData.xp = player.Xp;
        saveData.level = player.Level;
        saveData.iterium = player.Iterium;
        saveData.bulletLvl = player.BulletLvl;
        saveData.speedLvl = player.SpeedLvl;
        saveData.shieldLvl = player.ShieldLvl;

        print("Saving game data");
        fileSaveHandler.Save(saveData, saveFile);
        saveData.character = player.Character;
    }

    public void LoadGame()
    {
        print("Loading game data");
        saveData = fileSaveHandler.Load(saveFile);

        //Update data from load
        player.ProfileName = saveData.profileName;
        player.Bio = saveData.bio;
        player.Email = saveData.email;
        player.Xp = saveData.xp;
        player.Level = saveData.level;
        player.Iterium = saveData.iterium;
        player.BulletLvl = saveData.bulletLvl;
        player.SpeedLvl = saveData.speedLvl;
        player.ShieldLvl = saveData.shieldLvl;

        if (saveData.character != null)
        {
            player.Character = saveData.character;
        }
        else
        {
            player.Character = factions.Factions[2];
        }

        LoadLeaderboard();
    }

    public void NewGame()
    {
        print("Loading game defaults");
    }

    //Reset data to a new game state
    public void ResetGame()
    {
        //Player data
        player.ProfileName = "Player 1";
        player.Health = 100;
        player.Score = 0;
        player.Xp = 0;
        player.Level = 1;
        player.Iterium = 0;
        player.IteriumCollected = 0;
        player.SpeedLvl = 1;
        player.ShieldLvl = 1;
        player.BulletLvl = 1;
        player.Lives = 3;

        //AI data
        aiPlayer.ProfileName = "AI Player";
        aiPlayer.Health = 100;
        aiPlayer.Score = 0;
        aiPlayer.Xp = 0;
        aiPlayer.Level = 1;
        aiPlayer.Iterium = 0;
        aiPlayer.IteriumCollected = 0;
        aiPlayer.Lives = 3;
        aiPlayer.SpeedLvl = 1;
        aiPlayer.ShieldLvl = 1;
        aiPlayer.BulletLvl = 1;
        aiPlayer.Lives = 3;

        SaveGame();
    }

    //Reset data for a new arena battle
    public void NewArena()
    {
        //Player data
        player.Health = 100;
        player.Score = 0;
        player.IteriumCollected = 0;
        player.Lives = 3;

        //AI data
        aiPlayer.Health = 100;
        aiPlayer.Score = 0;
        aiPlayer.IteriumCollected = 0;
        aiPlayer.Lives = 3;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SceneUpgrade()
    {
        print("load upgrade scene");
        switch (player.Character.Country)
        {
            case "China":
                SceneManager.LoadScene("UpgradeChina");
                break;
            case "United States":
                SceneManager.LoadScene("UpgradeUSA");
                break;
            case "Russia":
                SceneManager.LoadScene("UpgradeUSSR");
                break;
        }
    }

    public void SpawnPlayer(float time)
    {
        StartCoroutine(SpawnPlayerOverTime(time));
    }

    public void SpawnAi(float time)
    {
        StartCoroutine(SpawnAiOverTime(time));
    }

    IEnumerator SpawnPlayerOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject ship = Instantiate(GameManager.Instance.player.Character.Ship.ShipPrefab);
        ship.transform.position = playerSpawner.position;
        ship.transform.rotation = playerSpawner.rotation;
        ship.transform.name = "Player";
        ship.transform.tag = "Player";
        player.Health = 100;
        aiTarget = ship;
    }

    IEnumerator SpawnAiOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        SelectAiPlayer();
        GameObject aiShip = Instantiate(GameManager.Instance.aiPlayer.Character.Ship.ShipPrefab);
        Destroy(aiShip.GetComponent<PlayerController>());
        Destroy(aiShip.GetComponent<InputManager>());
        aiShip.AddComponent<AIController>();
        aiShip.transform.position = aiSpawner.position;
        aiShip.transform.rotation = aiSpawner.rotation;
        aiShip.transform.name = "AI";
        aiShip.transform.tag = "AI";
        aiPlayer.Health = 100;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    //Save leaderboard to file
    public void SaveLeaderboard()
    {
        print("Saving leaderboard data");
        fileSaveHandler.SaveLeaderboard(leaderboard, saveFileLeaderboard);
    }

    //Load leaderboard from file
    public void LoadLeaderboard()
    {
        print("Loading leaderboard data");
        leaderboard = fileSaveHandler.LoadLeaderboard<LeaderboardItem>(saveFileLeaderboard);
    }

    //Add a new row to the leaderboard
    public void AddLeaderboardItem()
    {
        LeaderboardItem item = new LeaderboardItem();
        item.score = player.Score;
        item.date = System.DateTime.Now.Date.ToShortDateString();
        item.playerName = player.ProfileName;
        leaderboard.Add(item);
        SortLeaderboard();
        //Max leaderboard size
        if (leaderboard.Count > leaderboardSize)
        {
            leaderboard.Remove(leaderboard[leaderboard.Count - 1]);
        }
        SaveLeaderboard();
    }

    public void InitLeaderboard()
    {
        print("Init New Leaderboard");
        for (int i = 0; i < 10; i++)
        {
            LeaderboardItem item = new LeaderboardItem();
            item.score = (i + 1) * 10000;
            item.date = System.DateTime.Now.Date.ToShortDateString();
            item.playerName = "Imperial Xoid";
            leaderboard.Add(item);
        }
        SaveLeaderboard();
    }

    public void SortLeaderboard()
    {
        leaderboard = leaderboard.OrderByDescending(x => x.score).ToList();
    }

    public void CalculateXP()
    {
        if (player.Xp > xpLevelSteps * (player.Level + 1))
        {
            if (player.Level < maxLevel)
            {
                player.Level++;
            }
        }
    }

    public void CalculateIterium()
    {
        player.Iterium += player.IteriumCollected;
    }
}
