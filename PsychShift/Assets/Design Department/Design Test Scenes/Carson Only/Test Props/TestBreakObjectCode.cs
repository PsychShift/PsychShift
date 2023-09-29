using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBreakObjectCode : MonoBehaviour
{
public GameObject fractured;
public float breakForce;


    void Update()
    {
        if (Input.GetKeyDown ("f"))
            BreakTheThing();
    }

public void BreakTheThing(){
GameObject frac = Instantiate(fractured, transform.position, transform.rotation );

foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>()){
        Vector3 force = (rb.transform.position - transform. position) .normalized * breakForce;
        rb.AddForce(force);
    }
    Destroy(gameObject);
    }
}
