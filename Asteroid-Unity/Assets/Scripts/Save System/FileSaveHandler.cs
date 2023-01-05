using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// File system handler to write/read save data to/from a file
/// </summary>

public class FileSaveHandler : MonoBehaviour
{
    private string fileName;
    private string dirPath;

    public FileSaveHandler(string fileName, string dirPath)
    { 
        this.fileName = fileName;
        this.dirPath = dirPath;
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
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing save file: " + fullPath + "\n" + ex);
        }
    }

}
