using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardScript : MonoBehaviour
{
    public KeyCardModifier modifier;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KeyCardReq")
        {
            //open the noor
            

        }
    }
}
