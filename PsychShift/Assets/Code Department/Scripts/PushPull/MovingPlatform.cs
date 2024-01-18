using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if(tag == "Enemy" || tag == "Player")
        {
            other.transform.parent = transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        string tag = other.gameObject.tag;
        if(tag == "Enemy" || tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
