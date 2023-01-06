using UnityEngine;
using System;
using System.IO;

/// <summary>
/// File system handler to write/read save data to/from a file
/// </summary>

public class FileSaveHandler
{
    private string fileName;
    private string dirPath;   

    public FileSaveHandler(string dirPath, string fileName)
    {
        this.fileName = fileName;      
        this.dirPath = dirPath;
        Debug.Log("Save Folder: " + this.dirPath);
    }

    public SaveData Load()
    {
        string fullPath = Path.Combine(dirPath, fileName);

        SaveData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = null;
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<SaveData>(dataToLoad);
                Debug.Log("Successfully Loaded File");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading save file: " + fullPath + "\n" + ex);
            }
        }
        
        return loadData;
    }

    public void Save(SaveData saveData)
    {
        string fullPath = Path.Combine(dirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToSave = JsonUtility.ToJson(saveData, true);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(dataToSave);
                }
            }
            Debug.Log("Successfully Saved File");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing save file: " + fullPath + "\n" + ex);
        }
    }

}
