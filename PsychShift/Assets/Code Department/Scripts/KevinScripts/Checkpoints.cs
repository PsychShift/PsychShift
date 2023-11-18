using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    public PlayerStateMachine playerRef;
    private bool checkPointHit;

    // Start is called before the first frame update
    void Start()
    {
        if(checkPointHit==true)
        {
            playerRef.tempCharacter.transform.position = transform.position; 
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            checkPointHit = true; 
            Debug.Log("Checkpoint it"); 
        }
    }


}
