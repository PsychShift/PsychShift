using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GregActivate : MonoBehaviour
{
    public GameObject gregToTurnOnOFF;
    private Transform effectTransform;
    public ParticleSystem OnEffect;
    public ParticleSystem OffEffect;
    public AudioSource gregAudioSource;
    public AudioClip gregSpawnSound;
    private bool Instantiated;
    /* private void Awake() {
        effectTransform = gregToTurnOnOFF.transform;
    } */
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.layer == 15)
        {
            
            if(gregToTurnOnOFF!=null && Instantiated==false)
                gregToTurnOnOFF.SetActive(true);
            if(gregAudioSource!=null && Instantiated==false)
                gregAudioSource.PlayOneShot(gregSpawnSound);
            if(OnEffect!=null&& Instantiated==false)
            {
                effectTransform = gregToTurnOnOFF.transform;
                Instantiate(OnEffect, gregToTurnOnOFF.transform);
                Instantiated = true;
            }
            
                
            
            //PlayerInput.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.layer == 15)
        {
            
            if(gregToTurnOnOFF!=null && Instantiated == true)
                gregToTurnOnOFF.SetActive(false);
            if(gregAudioSource!=null && Instantiated == true)
                gregAudioSource.PlayOneShot(gregSpawnSound);
            if(OffEffect!=null && Instantiated == true)
            {
                Debug.Log("Why no spawn");
                Instantiate(OffEffect, effectTransform);
                Instantiated = false;
            }
            
                
                //if(Time.timeScale==1 && isReading == false)
            //PlayerInput.enabled = false;
        }
    }
}
