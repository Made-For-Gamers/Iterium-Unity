using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton manager to manage static data and methods
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Save Game")]
    [SerializeField] private string fileName;

    [Header("Characters")]
    public SO_Player player;
    public SO_Player aiPlayer;
    public SO_Player npcPlayer;
    public SO_Factions factions;

    public SO_GameObjects crystals;
    public int iteriumChance = 20;

    [HideInInspector] public SaveData saveData;
    private FileSaveHandler fileSaveHandler;


    private void Start()
    {
        this.fileSaveHandler = new FileSaveHandler(Application.persistentDataPath, fileName);
        LoadGame();
        SelectAiPlayer();
        ResetGame();
        player.Iterium = 1000;
        player.SpeedLvl = 0;
        player.ShieldLvl = 0;
        player.BulletLvl = 0;
    }

    private void SelectAiPlayer()
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        aiPlayer.Character = factions.Factions[rnd];
    }

    public void SaveGame()
    {
        print("Saving game data");
        fileSaveHandler.Save(saveData);
    }

    public void LoadGame()
    {
        print("Loading game data");
        saveData = fileSaveHandler.Load();

        if (saveData == null)
        {
            NewGame();
        }

        //Update player data with game load values
        player.Score = saveData.score;
        player.Xp = saveData.xp;
        player.ShieldLvl = saveData.shieldLvl;
        player.BulletLvl = saveData.bulletLvl;
        player.SpeedLvl = saveData.speedLvl;
        player.Iterium = saveData.iterium;
        if (saveData.character != null)
        {
            player.Character = saveData.character;
        }
        else
        {
            player.Character = factions.Factions[2];
        }
    }

    public void NewGame()
    {
        print("Loading game defaults");
        saveData = new SaveData();
    }

    public void ResetGame()
    {
        player.Health = 100;
        player.Score = 0;
        aiPlayer.Health = 100;
        aiPlayer.Score = 0;

    }

    private void OnApplicationQuit()
    {
      //  SaveGame();
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

}
