using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Guns;
using Guns.Stats;

public class EnemyCSVConverter
{
    private static readonly string EnemyCSVPath = "/Editor/CSV/EnemyCSV.csv";
    private static readonly string BaseGunFolder = "Assets/Guns/";

    
    //private const string end = "~";
    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + EnemyCSVPath);

        for(int i = 2; i < 11; i++) // if we change the number of weapons, this value needs to change
        {
            string[] splitData = allLines[i].Split(',');
            string gunFolder = BaseGunFolder + splitData[0];

            string[] GUID = AssetDatabase.FindAssets("l:Weapon", new string[] { gunFolder });
            string gunAssetPath = AssetDatabase.GUIDToAssetPath(GUID[0]);

            GunScriptableObject gun = AssetDatabase.LoadAssetAtPath<GunScriptableObject>(gunAssetPath);
            DamageConfigScriptableObject damageConfig = gun.DamageConfig;
            AmmoConfigScriptableObject ammoConfig = gun.AmmoConfig;
            ShootConfigScriptableObject shootConfig = gun.ShootConfig;
            CharacterStatsScriptableObject characterConfig = gun.CharacterConfig;

            damageConfig.DamageCurve.constant = float.Parse(splitData[1]);
            damageConfig.CritModifier = float.Parse(splitData[2]);

            shootConfig.FireRate = float.Parse(splitData[3]);

            int ammo = int.Parse(splitData[4]);
            ammoConfig.MaxAmmo = ammo;
            ammoConfig.ClipSize = ammo;
            ammoConfig.CurrentClipAmmo = ammo;

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

            characterConfig.Health = float.Parse(splitData[5]);
            characterConfig.WalkMoveSpeed = float.Parse(splitData[6]);
            characterConfig.WallMoveSpeed = float.Parse(splitData[7]);
            characterConfig.JumpForce =  float.Parse(splitData[8]);
            characterConfig.WallJumpForce = float.Parse(splitData[9]);
            
        }

        AssetDatabase.SaveAssets();
    }
}
