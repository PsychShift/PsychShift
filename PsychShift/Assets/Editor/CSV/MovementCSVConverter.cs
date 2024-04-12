using System.IO;
using UnityEngine;
using UnityEditor;
using Guns;
using Guns.Stats;

public class MovementCSVConverter
{
    private static readonly string MovementCSVPath = "/Editor/CSV/EnemyMovementCSV.csv";
    private static readonly string BaseGunFolder = "Assets/Guns/";

    
    //private const string end = "~";
    [MenuItem("Utilities/Generate Enemy Movement Data")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + MovementCSVPath);

        for(int i = 1; i < 10; i++) // if we change the number of weapons, this value needs to change
        {
            string[] splitData = allLines[i].Split(',');
            string gunFolder = BaseGunFolder + splitData[0];
            string[] GUID = AssetDatabase.FindAssets("l:Weapon", new string[] { gunFolder });
            string gunAssetPath = AssetDatabase.GUIDToAssetPath(GUID[0]);

            GunScriptableObject gun = AssetDatabase.LoadAssetAtPath<GunScriptableObject>(gunAssetPath);
            CharacterStatsScriptableObject characterConfig = gun.CharacterConfig;

            /* if(characterConfig == null)
            {
                characterConfig = ScriptableObject.CreateInstance<CharacterStatsScriptableObject>();
                characterConfig.Health = float.Parse(splitData[5]);
                characterConfig.WalkMoveSpeed = float.Parse(splitData[6]);
                characterConfig.WallMoveSpeed = float.Parse(splitData[7]);
                characterConfig.JumpForce =  float.Parse(splitData[8]);
                characterConfig.WallJumpForce = float.Parse(splitData[9]);
                Debug.Log(gunFolder);
                AssetDatabase.CreateAsset(characterConfig, $"{gunFolder}{splitData[0]}CharacterConfig.asset");
                gun.CharacterConfig = characterConfig;
            } */

            characterConfig.WalkMoveSpeed = float.Parse(splitData[1]);
            characterConfig.WallMoveSpeed = float.Parse(splitData[2]);
            characterConfig.JumpForce =  float.Parse(splitData[3]);
            characterConfig.WallJumpForce = float.Parse(splitData[4]);
            
        }

        AssetDatabase.SaveAssets();
    }
}

