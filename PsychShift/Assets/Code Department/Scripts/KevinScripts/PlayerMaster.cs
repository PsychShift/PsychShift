
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
    private static PlayerMaster _instance;
    private static readonly object _lock = new object();

    public static PlayerMaster Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
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
                }
            }
            return _instance;
        }
    }

    [SerializeField] private bool isMenu;
    [HideInInspector] public GameObject currentChar;
    //public GameObject charAtLastCheck;
    [HideInInspector]public GameObject charDouble;
    private PlayerStateMachine playerStateMachine;
    public Player.CharacterInfo charInfo => PlayerStateMachine.Instance == null ? null : PlayerStateMachine.Instance.currentCharacter;
    public Transform checkPointLocation;
    public void SetIsMenu(bool menu)
    {
        isMenu = menu;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        playerStateMachine = PlayerStateMachine.Instance;
        if(!isMenu)
        {
            Load();
        }
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
        

        SaveObject saveObject = new(scene, checkPoint, health, aIAgression, enemyType, gunType, modifiers);

        string json = JsonUtility.ToJson(saveObject);
        Debug.Log(json);
        SaveSystem.Save(json, "Saves", "Save");
    }


    // loading, for enemy prefab use GameAssets.Instance.EnemyPrefab
    public void Load()
    {
        string json = SaveSystem.Load("Saves", "Save");
        if(json == null)
        {
            //PlayerStateMachine.Instance.enabled = true;
            Debug.Log("waaaaaaaaaaaaa");
            return;
        }
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
        Debug.Log(saveObject);
        // if not unity editor, load the scene from the file
        SceneManager.LoadScene(saveObject.Scene, LoadSceneMode.Additive);
        Debug.Log("scene loaded");
        GameObject Enemy = Instantiate(GameAssets.Instance.GetEnemyPrefab());
        PlayerStateMachine.Instance.tempCharacter = Enemy;
        EnemyBrainSelector brainSelector = Enemy.GetComponent<EnemyBrainSelector>();
        GunScriptableObject gun = GameAssets.Instance.GetGun(saveObject.GunType);
        brainSelector.SwapBrain(gun, saveObject.EnemyType, saveObject.Modifiers, saveObject.AIAgression);
        
        PlayerStateMachine.Instance.enabled = true;
        Debug.Log("done");
    }

    public void StartNew()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, /* SAVE_FOLDER, */ "Saves", "Save" + ".txt");
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
