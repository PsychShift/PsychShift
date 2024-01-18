using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string directory = "/SaveData/";
    public static string filename = "MyData.txt";

    public static void Save(SaveObject saveObject)
    {
        string dir = Application.persistentDataPath + directory;

        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(saveObject);
            File.WriteAllText(dir + filename, json);
    }
    public static SaveObject Load()
    {
        string fullPath = Application.persistentDataPath + directory + filename;
        SaveObject saveObject = new SaveObject();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            saveObject = JsonUtility.FromJson<SaveObject>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }
        return saveObject;
    }
}

