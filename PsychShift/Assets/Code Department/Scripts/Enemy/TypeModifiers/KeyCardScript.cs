using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardScript : MonoBehaviour
{
    public KeyCardModifier modifier;
    private bool wasPlayer;
    public GameObject KeyCardUI;
    private bool alreadyActivated;

    private void Start() {
            //activate UI 
    }
    private void Update() {
        if(this.tag == "Player" && alreadyActivated == false)
        {
            wasPlayer = true;
            KeyCardUI.SetActive(true);
            alreadyActivated = true;
        }
        else if(this.tag != "Player" && wasPlayer == true)
        {
            wasPlayer = false;
            KeyCardUI.SetActive(false);
            alreadyActivated = false;
        }
        //deactivate UI 
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OPEN");
        if(other.tag == "KeyCardReq")
        {
            
            //open the noor
            other.GetComponent<KeyCardDoor>().OpenDaNoor();
        }
    }
}
