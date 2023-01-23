using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Singleton manager to manage static data and methods
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Save Game")]
    [SerializeField] private string saveFile;
    [SerializeField] private string saveFileLeaderboard;
    [SerializeField] private int leaderboardSize = 50;


    [Header("Characters")]
    public SO_Player player;
    public SO_Player aiPlayer;
    public SO_Player npcPlayer;
    public SO_Factions factions;
    public float deathRespawnTime = 4f;

    [Header("Iterium Crystals")]
    public SO_GameObjects iterium;
    public int iteriumChance = 20;

    //Spawn points
    [HideInInspector] public Transform playerSpawner;
    [HideInInspector] public Transform aiSpawner;
    [HideInInspector] public SaveData saveData = new SaveData();
    [HideInInspector] public List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();
    [HideInInspector] public GameObject aiTarget;

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
        print("Saving game data");
        saveData.profileName = player.ProfileName;
        saveData.character = player.Character;
        saveData.iterium = player.Iterium;
        saveData.bulletLvl = player.BulletLvl;
        saveData.speedLvl = player.SpeedLvl;
        saveData.shieldLvl = player.ShieldLvl;
        fileSaveHandler.Save(saveData, saveFile);
    }

    public void LoadGame()
    {
        print("Loading game data");
        saveData = fileSaveHandler.Load(saveFile);

        //if (saveData == null)
        //{
        //    NewGame();
        //}

        player.Xp = saveData.xp;
        player.ShieldLvl = saveData.shieldLvl;
        player.BulletLvl = saveData.bulletLvl;
        player.SpeedLvl = saveData.speedLvl;
        player.Iterium = saveData.iterium;
        player.ProfileName = saveData.profileName;
       
        if (saveData.character != null)
        {
            player.Character = saveData.character;
        }
        else
        {
            player.Character = factions.Factions[2];
        }

        print("Loading leaderboard data");
        leaderboard = fileSaveHandler.LoadLeaderboard<LeaderboardItem>(saveFileLeaderboard);
        SortLeaderboard();
    }

    public void NewGame()
    {
        print("Loading game defaults");
    }

    //Reset data to new game state
    public void ResetGame()
    {
        //Player data
        player.Health = 100;
        player.Score = 0;
        player.Iterium = 1000;
        player.IteriumCollected = 0;
        player.SpeedLvl = 1;
        player.ShieldLvl = 1;
        player.BulletLvl = 1;
        player.Lives = 3;

        //AI data
        aiPlayer.Health = 100;
        aiPlayer.Score = 0;
        aiPlayer.Iterium = 0;
        aiPlayer.IteriumCollected = 0;
        aiPlayer.Lives = 3;
        aiPlayer.SpeedLvl = 1;
        aiPlayer.ShieldLvl = 1;
        aiPlayer.BulletLvl = 1;
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
        SelectAiPlayer();
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

    public void AddLeaderboardItem()
    { 
        LeaderboardItem item = new LeaderboardItem();
        item.score = player.Score;
        item.date = System.DateTime.Now.Date.ToShortDateString();
        item.playerName = player.ProfileName;
        leaderboard.Add(item);
        SortLeaderboard();
        if (leaderboard.Count > leaderboardSize)
        {
            leaderboard.Remove(leaderboard[leaderboard.Count-1]);
            print("Removing last row from leaderboard");
        }
        print("Saving leaderboard data");
        fileSaveHandler.SaveLeaderboard(leaderboard, saveFileLeaderboard);
    }

    public void SortLeaderboard()
    {
        leaderboard = leaderboard.OrderByDescending(x => x.score).ToList();
    }

}
