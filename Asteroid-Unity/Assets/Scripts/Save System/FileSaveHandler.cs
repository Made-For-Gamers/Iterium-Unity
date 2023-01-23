using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// File system handler to write/read save data to/from a file, additional handler can be created for cloud saves etc
/// </summary>

public class FileSaveHandler
{

    private string dirPath;
    string fullPath;

    //WegGL plugin - force browser IndexDB sync on a save
    [DllImport("__Internal")]
    private static extern void syncSave();

    //WegGL plugin - browser alert popup for errors etc
    [DllImport("__Internal")]
    private static extern void alert(string message);

    public FileSaveHandler(string dirPath)
    {
        //Set the path for loading and saving
        this.dirPath = dirPath;
        Debug.Log("Save Folder: " + this.dirPath);
    }

    //Load player data from a json file
    public SaveData Load(string fileName)
    {
        fullPath = Path.Combine(dirPath, fileName);

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

    //Save player game data to a json file
    public void Save(SaveData saveData, string fileName)
    {
        fullPath = Path.Combine(dirPath, fileName);
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
            Debug.Log("Successfully saved file");
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

    //Load the leaderboard List<> from file using the JsonHelp wrapper class
    public List<T> LoadLeaderboard<T>(string fileName)
    {
        fullPath = Path.Combine(dirPath, fileName);
        List<T> newList;
        string dataToLoad = null;
        if (File.Exists(fullPath))
        {
            try
            {
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }
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
        if (string.IsNullOrEmpty(dataToLoad) || dataToLoad == "{}")
        {
            return new List<T>();
        }
        else
        {
            Debug.Log("Successfully Loaded File"); 
            newList = JsonHelper.FromJson<T>(dataToLoad).ToList();
            return newList;
        }
    }

    //Save the leaderboard List<> to a file using the JsonHelp wrapper class
    public void SaveLeaderboard<T>(List<T> saveData, string fileName)
    {
        fullPath = Path.Combine(dirPath, fileName);
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }
            string dataToSave = JsonHelper.ToJson<T>(saveData.ToArray(), true);

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
            Debug.Log("Successfully saved leaderboard file");
        }
        catch (Exception ex)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                alert("Error writing leaderboard file: " + fullPath + "\n" + ex);
            }
            else
            {
                Debug.LogError("Error writing Leaderboard file: " + fullPath + "\n" + ex);
            }
        }
    }
}

//Wrapper class to wrap Lists<> so that they can be serialised by JsonUtility
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
