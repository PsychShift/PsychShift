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

    private void Awake() 
    {
            
    }
    private void Update() 
    {
        if(howManyComplete==howManyAreThere)
        {
            //Do the thing here
            //Hard code for now
            Rigidbody floorBoxRef = dropFloor.GetComponent<Rigidbody>();
            floorBoxRef.useGravity = true;

        }
    }

    public void AddPuzzle()
    {
        howManyComplete++;
    }


    
    
}
