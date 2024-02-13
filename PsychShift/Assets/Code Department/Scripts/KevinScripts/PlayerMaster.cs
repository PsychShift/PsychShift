
using System;
using System.IO;
using BrainSwapSaving;
using Guns;
using Guns.Demo;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMaster : MonoBehaviour
{
    private static PlayerMaster _instance = null;
    //private static readonly object _lock = new object();

    public static PlayerMaster Instance
    {
        get
        {
            /* if (_instance == null)
            {

                if (_instance == null)
                {
                    var obj = new GameObject();
                    obj.name = "PlayerMaster";
                    obj.transform.localPosition = Vector3.zero;
                    obj.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
                    _instance = obj.AddComponent<PlayerMaster>();
                    DontDestroyOnLoad(obj);
                }
                
            } */
            return _instance;
        }
    }

    [SerializeField] private bool isMenu;
    [HideInInspector] public GameObject currentChar;
    //public GameObject charAtLastCheck;
    [HideInInspector]public GameObject charDouble;
    //private PlayerStateMachine playerStateMachine;
    public Player.CharacterInfo charInfo => PlayerStateMachine.Instance == null ? null : PlayerStateMachine.Instance.currentCharacter;
    public Transform checkPointLocation;
    private bool isLoadingSceneFromLoadMethod = false;
    private SaveObject loadedInfo;
    public void SetIsMenu(bool menu)
    {
        isMenu = menu;
    }
    // Start is called before the first frame update


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void SetCheckPoint(Transform position)//Call this with transform.zero to reset checkpoint when player quits
    {
        checkPointLocation = position;
        PlayerStateMachine.Instance.SetLocation(checkPointLocation);

        SaveInfo();
    }


    private void SaveInfo()
    {
        // use transform, current character info
        Vector3 checkPoint = checkPointLocation.position;
        int health = charInfo.enemyHealth.CurrentHealth;

        AIAgression aIAgression = charInfo.enemyBrain.agression;
        // we need enemy type as well
        EnemyBrainSelector brainSelector = charInfo.characterContainer.GetComponent<EnemyBrainSelector>();
        EBrainType enemyType = brainSelector.enemyType;
        GunType gunType = brainSelector.gunType.Type;
        EEnemyModifier[] modifiers = brainSelector.modifiers;
        int scene = 0;
 
        scene = SceneManager.GetActiveScene().buildIndex;
        

        SaveObject loadedInfo = new(scene, checkPoint, health, aIAgression, enemyType, gunType, modifiers);

        string json = JsonUtility.ToJson(loadedInfo);
        Debug.Log(json);
        SaveSystem.Save(json, "Saves", "Save");
    }


    // loading, for enemy prefab use GameAssets.Instance.EnemyPrefab
    public void Load()
    {
        string json = SaveSystem.Load("Saves", "Save");
        if(json == null)
        {
            Debug.Log("waaaaaaaaaaaaa");
            loadedInfo = null;
            return;
        }
        loadedInfo = JsonUtility.FromJson<SaveObject>(json);
        isLoadingSceneFromLoadMethod = true;
        SceneManager.LoadScene(loadedInfo.Scene, LoadSceneMode.Single);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only perform actions if the scene was loaded from the Load method
        if (isLoadingSceneFromLoadMethod && scene.buildIndex == loadedInfo.Scene)
        {
            // Reset the flag after performing the actions
            isLoadingSceneFromLoadMethod = false;

            GameObject Enemy = Instantiate(GameAssets.Instance.GetEnemyPrefab());
            Enemy.transform.position = loadedInfo.Savepoint;
            Enemy.name = "RespawnEnemy";
            EnemyBrainSelector brainSelector = Enemy.GetComponent<EnemyBrainSelector>();
            GunScriptableObject gun = GameAssets.Instance.GetGun(loadedInfo.GunType);
            brainSelector.SwapBrain(gun, loadedInfo.EnemyType, loadedInfo.Modifiers, loadedInfo.AIAgression);
            PlayerStateMachine.Instance.tempCharacter = Enemy;
        }
        PlayerStateMachine.Instance.Load();
    }

    public void StartNew()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, /* SAVE_FOLDER, */ "Saves", "Save" + ".txt");
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
