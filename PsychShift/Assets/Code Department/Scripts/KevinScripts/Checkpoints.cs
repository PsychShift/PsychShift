using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    private bool hitCheck;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player"&& !hitCheck)
        {
            Debug.Log("Checkpoint hit");
            hitCheck = true;
            PlayerMaster.Instance.SetCheckPoint(gameObject.transform, other.gameObject);
            

            
        }
    }


}
