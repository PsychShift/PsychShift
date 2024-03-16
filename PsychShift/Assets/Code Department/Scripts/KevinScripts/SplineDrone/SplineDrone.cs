using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDrone : MonoBehaviour
{
    public float MaxRayCastDistance = 20f;
    // Start is called before the first frame update\
    public SplineAnimate splineRef;
    public Collider triggerBoxToStartDroneAgain;
    private void Update() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRayCastDistance, 7))
        {
            // Check if the object hit by the ray is on the vault layer
            HitBarrier();
            
        }   
    }
    private void HitBarrier()
    {
        splineRef.enabled = false;
        triggerBoxToStartDroneAgain.enabled = true;

    }

    private void OnTriggerEnter(Collider other) 
    {

    }
}
