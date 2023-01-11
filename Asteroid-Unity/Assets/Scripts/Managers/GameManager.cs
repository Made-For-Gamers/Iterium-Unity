using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton manager to manage static data and methods
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Save Game Settings")]
    [SerializeField] private string fileName;

    [Header("Characters")]
    public SO_Player player;
    public SO_Player npc;
    public SO_Factions factions;

    [HideInInspector] public SaveData saveData;
    [HideInInspector] public SO_Character aiCharacter;
    private FileSaveHandler fileSaveHandler;
      

    private void Start()
    {        
        this.fileSaveHandler = new FileSaveHandler(Application.persistentDataPath, fileName);
        LoadGame();
        SelectAiPlayer();
        ResetGame();
    }

    private void SelectAiPlayer()
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        aiCharacter = factions.Factions[rnd];
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
    }

    private void OnApplicationQuit()
    {
        //SaveGame();     
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
