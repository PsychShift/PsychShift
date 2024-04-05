using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDrone : MonoBehaviour
{
    //public float MaxRayCastDistance = 20f;
    // Start is called before the first frame update\
    public SplineAnimate splineRef;
    //public Collider triggerBoxToStartDroneAgain;
    private void Awake() 
    {
        splineRef.Pause();
    }
    

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == 15)
       {
        Debug.Log("Play splien");
            splineRef.Play();
       }        
    }
}
