using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

/// <summary>
/// File system handler to write/read save data to/from a file, additional handler can be created for cloud saves etc
/// </summary>

public class FileSaveHandler
{
    private string fileName;
    private string dirPath;

    //WegGL plugin - force browser IndexDB sync on a save
    [DllImport("__Internal")]
    private static extern void syncSave();

    //WegGL plugin - browser alert popup for errors etc
    [DllImport("__Internal")]
    private static extern void alert(string message);

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
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
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
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    alert("Error loading save file: " + fullPath + "\n" + ex);
                }
                else
                {
                    Debug.LogError("Error loading save file: " + fullPath + "\n" + ex);
                }
            }
        }
        return loadData;
    }

    public void Save(SaveData saveData)
    {
        string fullPath = Path.Combine(dirPath, fileName);
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }
            string dataToSave = JsonUtility.ToJson(saveData, true);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(dataToSave);
                }
            }
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                syncSave();             
            }
            Debug.Log("Successfully Saved File");
        }
        catch (Exception ex)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                alert("Error writing save file: " + fullPath + "\n" + ex);
            }
            else
            { 
             Debug.LogError("Error writing save file: " + fullPath + "\n" + ex);
            }  
        }
    }

}
