using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BrainSwapSaving
{
    public static class SaveSystem
    {
        // Define your save folder in the Resources folder
        private static readonly string SAVE_FOLDER = "";

        public static void Save(string saveString, string folderName, string fileName)
        {
            // Create a new file in the save folder
            string fullPath = Path.Combine(Application.streamingAssetsPath, /* SAVE_FOLDER, */ folderName, fileName + ".txt");
            fullPath = fullPath.Replace('/', '\\');
            Debug.Log(fullPath);

            // Check if we're in the Unity Editor
            /* if (Application.isEditor)
            {
                // Construct the full path to the file in the Assets folder
                string assetsPath = Path.Combine(Application.dataPath, SAVE_FOLDER, folderName, fileName + ".txt");
                assetsPath = assetsPath.Replace('/', '\\');
                
                // Write the save string to the file in the Assets folder
                File.WriteAllText(assetsPath, saveString);
            } */
            // Write the save string to the file in the StreamingAssets folder
            File.WriteAllText(fullPath, saveString);

        }

        public static string Load(string folderName, string fileName)
        {
            // Construct the full path to the file in the StreamingAssets folder
            string fullPath = Path.Combine(Application.streamingAssetsPath, SAVE_FOLDER, folderName, fileName + ".txt");

            // Check if the file exists
            if (File.Exists(fullPath))
            {
                // Read the file and return its contents
                return File.ReadAllText(fullPath);
            }

            // Return null if the file doesn't exist
            return null;
        }
    }
    /* public static class SaveSystem
    {

        private static readonly string SAVE_FOLDER_Game = System.IO.Directory.GetCurrentDirectory() + "/Levels";
        private static readonly string SAVE_FOLDER_Editor = Application.dataPath + "/Code Department/Scripts/ModelSwapping";

        public static void Save(string saveString, string fileName, string filePath = "")
        {
            if(filePath.Length == 0)
                File.WriteAllText(SAVE_FOLDER_Editor + fileName + ".txt", saveString);
            else
                File.WriteAllText(filePath + fileName + ".txt", saveString);
        }

        public static string Load(string fileName, string filePath = "default")
        {
    #if UNITY_EDITOR
            string path = filePath == "default" ? SAVE_FOLDER_Editor : filePath;
            if(File.Exists(path + fileName + ".txt")) 
            {
                string saveString = File.ReadAllText(path + fileName + ".txt");
                return saveString;
            }
    #endif
            if (File.Exists(SAVE_FOLDER_Game + fileName + ".txt"))
            {
                string saveString = File.ReadAllText(SAVE_FOLDER_Game + fileName + ".txt");
                return saveString;
            }
            return null;
        }
    } */
}
