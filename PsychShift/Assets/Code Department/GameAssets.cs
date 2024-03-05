using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guns;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get 
        {
            if(instance == null) instance = Instantiate(Resources.Load<GameObject>("GameAssets").GetComponent<GameAssets>());
            return instance;
        }
    }


        [Header("Assets for the Resource folder")]
        [Header("Prefabs")]
        public GameObject EnemyPrefab;
        public List<GameObject> prefabs;
        public GameObject GetPrefab(string name)
        {
            return prefabs.FirstOrDefault(go => go.name == name);
        }
        public GameObject GetEnemyPrefab()
        {
            return EnemyPrefab;
        }

        public GameObject level1VCamPrefab;
        public GameObject level2VCamPrefab;
        public GameObject level3VCamPrefab;


        [Header("Materials")]
        public List<Material> materials;
        public Material GetMaterial(string name)
        {
            return materials.FirstOrDefault(ma => ma.name == name);
        }

        [Header("Guns")]
        public GunScriptableObject[] guns;
        public GunScriptableObject GetGun(GunType gunType)
        {
            return guns.FirstOrDefault(gun => gun.Type == gunType);
        }

        [Header("AIAgression")]
        public AIAgression[] Agressions;
}
