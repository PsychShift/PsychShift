using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataLog : MonoBehaviour
{
    public GameObject interactTextObject;
    public GameObject textObject;
    public GameObject picture;
    //public PlayerInput PlayerInput;
    public AudioClip audioLog;
    public AudioSource audioSource;
    private bool isReading;
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.layer == 15)
        {
            if(interactTextObject!=null)
                interactTextObject.SetActive(true);
            
            //PlayerInput.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.layer == 15)
        {
            if(interactTextObject!=null)
                interactTextObject.SetActive(false);
            if(textObject!= null)
                textObject.SetActive(false);
            if(audioSource!=null)
                audioSource.Stop();
            if(picture!=null)
                picture.SetActive(false);
            isReading = false;
                //if(Time.timeScale==1 && isReading == false)
            //PlayerInput.enabled = false;
        }
    }
    
    public void TextInteract()
    {
        //Debug.Log("here");
        if(isReading == false)
        {
            Debug.Log("read");
            //Time.timeScale = 0;
            if(textObject!=null)
                textObject.SetActive(true);
            if(audioSource!=null)
                audioSource.PlayOneShot(audioLog);
            if(picture!=null)
                picture.SetActive(true);
            isReading = true;
            //PlayerInput.enabled = true;
        }
        else
        {
            Debug.Log("DonRead");
            //Time.timeScale = 1;
            if(textObject!= null)
                textObject.SetActive(false);
            if(audioSource!=null)
                audioSource.Stop();
            if(picture!=null)
                picture.SetActive(false);
            isReading = false;
            //PlayerInput.enabled = false;
            
        }
            
    }
    private void OnDisable() 
    {
        isReading = false;
    }
}
