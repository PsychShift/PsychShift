using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardScript : MonoBehaviour
{
    private bool wasPlayer;
    private bool alreadyActivated;

    private void Start() {
            //activate UI 
    }
    private void Update() {

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KeyCardReq")
        { 
            //open the noor
            other.GetComponent<KeyCardDoor>().OpenDaNoor();
        }
    }

    public void SwapIn()
    {
        wasPlayer = true;
        KeycardUI.Instance.SetActive(true);
        alreadyActivated = true;
    }

    public void SwapOut()
    {
        wasPlayer = false;
        KeycardUI.Instance.SetActive(false);
        alreadyActivated = false;
    }
}
