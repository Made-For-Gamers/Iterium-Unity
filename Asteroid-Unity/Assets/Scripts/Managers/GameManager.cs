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
    [Header("Faction Upgrade Scenes")]
#if UNITY_EDITOR
    public UnityEditor.SceneAsset destinationSceneChn;
    public UnityEditor.SceneAsset destinationSceneUs;
    public UnityEditor.SceneAsset destinationSceneUssr;
    private void OnValidate()
    {
        if (destinationSceneChn != null)
        {
            upgradeChnScene = destinationSceneChn.name;
            upgradeUsScene = destinationSceneUs.name;
            UpgradeUssrScene = destinationSceneUssr.name;
        }
    }
#endif
    [HideInInspector] public string upgradeChnScene;
    [HideInInspector] public string upgradeUsScene;
    [HideInInspector] public string UpgradeUssrScene;

    [Header("Save Game")]
    [SerializeField] private string saveFile;
    [SerializeField] private string saveFileLeaderboard;
    [SerializeField] private int leaderboardSize = 50;

    [Header("Player Settings")]
    public SO_Player player;
    [HideInInspector] public GameObject targetPlayer;
    public SO_Factions factions;
    public float deathRespawnTime = 4f;
    public int xpLevelSteps = 1000;
    public int maxLevel = 50;

    [Header("AI Settings")]
    public SO_Player aiPlayer;
    public bool aiPermadeath;
    [HideInInspector] public GameObject targetAi;

    [Header("NPC Settings")]
    public SO_Player npcPlayer;
    [HideInInspector] public GameObject targetNpc;

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
        //Load Game Save
        this.fileSaveHandler = new FileSaveHandler(Application.persistentDataPath);
        LoadGame();

        //Events
        player.onChange_bulletLvl.AddListener(BulletLvlChanged);
        player.onChange_shieldLvl.AddListener(ShieldLvlChanged);
        player.onChange_speedLvl.AddListener(SpeedLvlChanged);
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
        saveData.bulletLvlUs = player.BulletLvlUs;
        saveData.speedLvlUs = player.SpeedLvlUs;
        saveData.shieldLvlUs = player.ShieldLvlUs;
        saveData.bulletLvlUssr = player.BulletLvlUssr;
        saveData.speedLvlUssr = player.SpeedLvlUssr;
        saveData.shieldLvlUssr = player.ShieldLvlUssr;
        saveData.bulletLvlChn = player.BulletLvlChn;
        saveData.speedLvlChn = player.SpeedLvlChn;
        saveData.shieldLvlChn = player.ShieldLvlChn;

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
        player.BulletLvlUs = saveData.bulletLvlUs;
        player.SpeedLvlUs = saveData.speedLvlUs;
        player.ShieldLvlUs = saveData.shieldLvlUs;
        player.BulletLvlUssr = saveData.bulletLvlUssr;
        player.SpeedLvlUssr = saveData.speedLvlUssr;
        player.ShieldLvlUssr = saveData.shieldLvlUssr;
        player.BulletLvlChn = saveData.bulletLvlChn;
        player.SpeedLvlChn = saveData.speedLvlChn;
        player.ShieldLvlChn = saveData.shieldLvlChn;

        if (saveData.character != null)
        {
            player.Character = saveData.character;
        }
        else
        {
            player.Character = factions.Factions[2];
        }

        LoadLeaderboard();
        UpgradeLevelSync();
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
        player.SpeedLvlUs = 1;
        player.ShieldLvlUs = 1;
        player.BulletLvlUs = 1;
        player.SpeedLvlUssr = 1;
        player.ShieldLvlUssr = 1;
        player.BulletLvlUssr = 1;
        player.SpeedLvlChn = 1;
        player.ShieldLvlChn = 1;
        player.BulletLvlChn = 1;
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
        aiPlayer.SpeedLvlUs = 1;
        aiPlayer.ShieldLvlUs = 1;
        aiPlayer.BulletLvlUs = 1;
        aiPlayer.SpeedLvlUssr = 1;
        aiPlayer.ShieldLvlUssr = 1;
        aiPlayer.BulletLvlUssr = 1;
        aiPlayer.SpeedLvlChn = 1;
        aiPlayer.ShieldLvlChn = 1;
        aiPlayer.BulletLvlChn = 1;
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
        switch (player.Character.Id)
        {
            case "chn":
                SceneManager.LoadScene(upgradeChnScene);
                break;
            case "us":
                SceneManager.LoadScene(upgradeUsScene);
                break;
            case "ussr":
                SceneManager.LoadScene(UpgradeUssrScene);
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
        targetPlayer = Instantiate(Instance.player.Character.Ship.ShipPrefab);
        targetPlayer.transform.position = RandomScreenPosition(playerSpawner);
        targetPlayer.transform.rotation = playerSpawner.rotation;
        targetPlayer.transform.name = "Player";
        targetPlayer.transform.tag = "Player";
        player.Health = 100;
    }

    public Vector3 RandomScreenPosition(Transform spawnPoint)
    {
        float x = Random.Range(-4f, 4f);
        float z = Random.Range(-6f, 6f);
        Vector3 position = new Vector3(spawnPoint.position.x + x, 0, spawnPoint.position.z + z);
        return position;
    }

    IEnumerator SpawnAiOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        SelectAiPlayer();
        targetAi = Instantiate(GameManager.Instance.aiPlayer.Character.Ship.ShipPrefab);
        Destroy(targetAi.GetComponent<PlayerController>());
        Destroy(targetAi.GetComponent<InputManager>());
        targetAi.AddComponent<AIController>();
        targetAi.transform.position = RandomScreenPosition(aiSpawner);
        targetAi.transform.rotation = aiSpawner.rotation;
        targetAi.transform.name = "AI";
        targetAi.transform.tag = "AI";
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

    private void BulletLvlChanged()
    {
        switch (GameManager.Instance.player.Character.Id)
        {
            case "chn":
                player.BulletLvlChn = player.BulletLvl;
                break;
            case "us":
                player.BulletLvlUs = player.BulletLvl;
                break;
            case "ussr":
                player.BulletLvlUssr = player.BulletLvl;
                break;
        }
    }

    private void ShieldLvlChanged()
    {
        switch (GameManager.Instance.player.Character.Id)
        {
            case "chn":
                player.ShieldLvlChn = player.ShieldLvl;
                break;
            case "us":
                player.ShieldLvlUs = player.ShieldLvl;
                break;
            case "ussr":
                player.ShieldLvlUssr = player.ShieldLvl;
                break;
        }

    }

    private void SpeedLvlChanged()
    {
        switch (GameManager.Instance.player.Character.Id)
        {
            case "chn":
                player.SpeedLvlChn = player.SpeedLvl;
                break;
            case "us":
                player.SpeedLvlUs = player.SpeedLvl;
                break;
            case "ussr":
                player.SpeedLvlUssr = player.SpeedLvl;
                break;
        }
    }

    public void UpgradeLevelSync()
    {
        switch (GameManager.Instance.player.Character.Id)
        {
            case "chn":
                player.BulletLvl = player.BulletLvlChn;
                player.ShieldLvl = player.ShieldLvlChn;
                player.SpeedLvl = player.SpeedLvlChn;
                break;
            case "us":
                player.BulletLvl = player.BulletLvlUs;
                player.ShieldLvl = player.ShieldLvlUs;
                player.SpeedLvl = player.SpeedLvlUs;
                break;
            case "ussr":
                player.BulletLvl = player.BulletLvlUssr;
                player.ShieldLvl = player.ShieldLvlUssr;
                player.SpeedLvl = player.SpeedLvlUssr;
                break;
        }
    }
}
