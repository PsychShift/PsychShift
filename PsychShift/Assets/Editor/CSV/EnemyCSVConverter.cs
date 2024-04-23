using System.IO;
using UnityEngine;
using UnityEditor;
using Guns;
using Guns.Stats;

public class EnemyCSVConverter
{
    private static readonly string EnemyCSVPath = "/Editor/CSV/EnemyCSV.csv";
    private static readonly string BaseGunFolder = "Assets/Guns/";

    
    //private const string end = "~";
    [MenuItem("Utilities/Generate Enemy Data")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + EnemyCSVPath);

        for(int i = 1; i < 10; i++) // if we change the number of weapons, this value needs to change
        {
            string[] splitData = allLines[i].Split(',');
            string gunFolder = BaseGunFolder + splitData[0];
            string[] GUID = AssetDatabase.FindAssets("l:Weapon", new string[] { gunFolder });
            string gunAssetPath = AssetDatabase.GUIDToAssetPath(GUID[0]);

            GunScriptableObject gun = AssetDatabase.LoadAssetAtPath<GunScriptableObject>(gunAssetPath);
            DamageConfigScriptableObject damageConfig = gun.DamageConfig;
            AmmoConfigScriptableObject ammoConfig = gun.AmmoConfig;
            Guns.ShootConfigScriptableObject shootConfig = gun.ShootConfig;
            CharacterStatsScriptableObject characterConfig = gun.CharacterConfig;

            float dmg = float.Parse(splitData[1]);
            float minDist = float.Parse(splitData[6]);
            float maxDist = float.Parse(splitData[7]);
            float ratio = float.Parse(splitData[8]);

            float slope = (dmg * ratio - dmg) / (maxDist - minDist); // Slope between Vector2 2 and Vector2 3

            Keyframe[] frames = new Keyframe[3]
            {
                new(0, dmg, 0, 0),
                new(minDist, dmg, 0, slope),
                new(maxDist, dmg * ratio, slope, 0)
            };
            damageConfig.DamageCurve.curve = new AnimationCurve(frames);
            damageConfig.CritModifier = float.Parse(splitData[2]);

            Debug.Log(gunFolder + damageConfig.DamageCurve.constant);
            shootConfig.FireRate = float.Parse(splitData[3]);
            shootConfig.BulletsPerShot = int.Parse(splitData[9]);

            int ammo = int.Parse(splitData[4]);
            ammoConfig.MaxAmmo = 0;
            ammoConfig.ClipSize = 0;
            ammoConfig.CurrentAmmo = 0;
            ammoConfig.CurrentClipAmmo = ammo;



            characterConfig.Health = float.Parse(splitData[5]);            
            EditorUtility.SetDirty(gun);
            EditorUtility.SetDirty(damageConfig);
            EditorUtility.SetDirty(shootConfig);
            EditorUtility.SetDirty(ammoConfig);
            EditorUtility.SetDirty(characterConfig);
            AssetDatabase.SaveAssets();
        }
    }
}

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