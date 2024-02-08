using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public PlayerStateMachine checkpointRest;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene("Outro"); 
            checkpointRest.SetLocation(transform);
        }
            

    }
}
