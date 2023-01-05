using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Singleton manager to manage static data
/// </summary>
public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    [Header("Save Game Settings")]
    [SerializeField] private string fileName;

    private SaveData saveData;
    private List<ISave> iSave;
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
    }

    private void Start()
    {
        fileSaveHandler = new FileSaveHandler(Application.persistentDataPath, fileName);
        iSave = GetSaveData();
        LoadGame();
    }

    public void SaveGame()
    {
        foreach (ISave saveItems in iSave)
        {
            saveItems.SaveData(ref saveData);
        }
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

    private List<ISave> GetSaveData()
    {
        IEnumerable<ISave> result = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        return new List<ISave>(result);
    }

}
