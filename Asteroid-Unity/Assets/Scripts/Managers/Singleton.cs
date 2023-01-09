using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Unity.Mathematics;

/// <summary>
/// Singleton manager to manage static data and methods
/// </summary>
public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    [Header("Save Game Settings")]
    [SerializeField] private string fileName;

    [Header("Characters")]
    public SO_Player player;
    public SO_Player npc;
    public SO_Character defaultCharacter;
    private SO_Ship playerAi;

    [HideInInspector] public SaveData saveData;
    private FileSaveHandler fileSaveHandler;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        this.fileSaveHandler = new FileSaveHandler(Application.persistentDataPath, fileName);
        LoadGame();

    }

    private void Start()
    {
        ResetGame();
        SelectAiPlayer();
    }

    private void SelectAiPlayer()
    {
        int rnd = UnityEngine.Random.Range(0,2);

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
        player.Iterium = saveData.iterium;      
        if (saveData.character != null)
        {
            player.Character = saveData.character;
        }
        else
        {
            player.Character = defaultCharacter;
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

}
