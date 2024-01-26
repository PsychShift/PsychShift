using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BrainSwapSaving
{
    public static class SaveSystem
    {

        private static readonly string SAVE_FOLDER_Game = System.IO.Directory.GetCurrentDirectory() + "/Levels";
        private static readonly string SAVE_FOLDER_Editor = Application.dataPath + "/Code Department/Scripts/ModelSwapping";

        public static void Save(string saveString, string fileName)
        {
            File.WriteAllText(SAVE_FOLDER_Editor + fileName + ".txt", saveString);
        }

        public static string Load(string fileName)
        {
    #if UNITY_EDITOR
            if(File.Exists(SAVE_FOLDER_Editor + fileName + ".txt")) 
            {
                string saveString = File.ReadAllText(SAVE_FOLDER_Editor + fileName + ".txt");
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
    }
}
