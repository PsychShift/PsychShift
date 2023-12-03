using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTracker : MonoBehaviour
{
    //Detects how many puzzles are completed in an encounter
    //give a ref to all god boxes
    [TextArea]
    public string Notes = "Attach this script to each encounter to track progress in UI";

    [HideInInspector]
    public int howManyComplete;
    public int howManyAreThere;
    public GameObject dropFloor;
    AudioSource bossFloorAudio;
    public AudioClip bossSiren;
    public GameObject fakeWarden;
    public GameObject realWarden;

    private void Awake() 
    {
        bossFloorAudio = gameObject.GetComponent<AudioSource>();
        realWarden.SetActive(false);
    }
    private void Update() 
    {
        if(howManyComplete==howManyAreThere)
        {
            //Do the thing here
            //Hard code for now
            Animator floorBoxRef = dropFloor.GetComponent<Animator>();
            floorBoxRef.SetBool("Move", true);
            bossFloorAudio.PlayOneShot(bossSiren);
            //disable anim
            fakeWarden.SetActive(false);
            realWarden.SetActive(true);


        }
    }

    public void AddPuzzle()
    {
        howManyComplete++;
    }


    
    
}
