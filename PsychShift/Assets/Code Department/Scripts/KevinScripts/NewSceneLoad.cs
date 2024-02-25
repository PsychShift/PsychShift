using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoad : MonoBehaviour
{
    public bool trevor;
    public bool noUSEyet;
    public PlayerStateMachine resetCheckpoint;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            if(trevor)
            {
                PlayerMaster.Instance.StartNew();
                resetCheckpoint.SetLocation(transform);
                SceneManager.LoadScene("TREVOR ROOM");
            }
                
        }
    }
}
