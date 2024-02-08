using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScriptLevel2 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerStateMachine checkpointRest;
    void OnEnable()
    {
        SceneManager.LoadScene("WinScreen");
        checkpointRest.SetLocation(transform);
    }
}
