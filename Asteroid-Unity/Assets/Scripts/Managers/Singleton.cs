using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Singleton manager to manage static data
/// </summary>
public class Singleton : MonoBehaviour
{
    public static Singleton Instance;
    private SaveData saveData;
    private List<ISave> iSave;

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
    }

    private void Start()
    {
        iSave = FindSaveData();
        LoadGame();
    }

    public void SaveGame()
    {
        foreach (ISave saveItems in iSave)
        {
            saveItems.SaveData(ref saveData);
        }
    }

    public void LoadGame()
    {
        print("Loading game data");
        if (saveData == null)
        {           
            NewGame();
        }

        foreach (ISave saveItems in iSave)
        {
            saveItems.LoadData(saveData);
        }
    }

    public void NewGame()
    {
        print("Loading game defaults");
        saveData = new SaveData();
    }

    private void OnApplicationQuit()
    {
        print("Saving game data");
        SaveGame();
        print(saveData.score);
    }

    private List<ISave> FindSaveData()
    {
        IEnumerable<ISave> result = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        return new List<ISave>(result);
    }

}
