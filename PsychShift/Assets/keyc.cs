using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keyc : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerStateMachine checkpointRest;
    void Awake()
    {
        SceneManager.LoadScene("WinScreen");
        checkpointRest.PleaseSetLocationGODPLEASE(this.transform, true);
    }
}
