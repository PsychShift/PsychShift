using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMaster : MonoBehaviour
{
    private static PlayerMaster _Instance;
    public static PlayerMaster Instance{
        get
        {
            return _Instance;
        } 
        set
        {
            _Instance = value;
        }
    }//makes sure there's only one player master
    private bool isDeath;
    [HideInInspector] public GameObject currentChar;
    //public GameObject charAtLastCheck;
    [HideInInspector]public GameObject charDouble;
    public static Transform checkPointLocationTWO;
    public Transform checkPointLocation;
    public PlayerStateMachine playerRef;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("ChecPointLTwo "+ checkPointLocationTWO.position);
        if(checkPointLocationTWO!= null)
        {
            checkPointLocation = checkPointLocationTWO;
        }
            
        if(Instance == null)
        {
            Instance = this;//sets value
        }
        else
        {
            Destroy(this);//if more than one destroy 
            return;
        }
        //DontDestroyOnLoad(gameObject);//Won't care for scene reloading
        //charDouble = charAtLastCheck;
        //SceneManager.sceneLoaded+= OnSceneLoaded;
    }

    /* void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    } */

    // Update is called once per frame
    /* void Update()
    {
        if(charAtLastCheck!= null)
        {
            
        }
    } */
    public void SetCheckPoint(Transform position, GameObject character)
    {
        Debug.Log("Info set PM");
        checkPointLocation= position;
        checkPointLocationTWO=checkPointLocation;
        Debug.Log("CheckLoc "+ checkPointLocation.position);
        playerRef.PleaseSetLocationGODPLEASE(checkPointLocation);
        //charAtLastCheck = character;
        

    }
}
