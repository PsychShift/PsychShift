using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classified : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        Debug.Log("I'm in your walls");
        Debug.LogWarning("I'm in your walls");
        Debug.LogError("I'm in your walls");
        Debug.Log(this.gameObject.name);
        //Debug.LogException("I'm in your walls");
    }
}
