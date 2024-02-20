using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript: MonoBehaviour
{
    //script alternates between on and off
    //Will be on when in the level
    //will be off when outside the level
    //determined by the trigger boxes.
    // Start is called before the first frame update
    public BlackHole onOffBHRef;
    public bool exitTrigger;
    private bool activated;
    private void OnTriggerExit(Collider other) 
    {
        //Activate blackhole once player exits trigger
        if(exitTrigger== false&& activated== false && other.tag == "Player")
        {
            onOffBHRef.StartTheBH();
            activated = true;
        }
            
        else
            onOffBHRef.EmergencyOff();
    }
}
