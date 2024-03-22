using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classified : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        Debug.Log("BeepBeepBoop");
        Debug.LogWarning("BeepBeepBoop");
        Debug.LogError("BeepBeepBoop");
        Debug.Log(this.gameObject.name);
        //Debug.LogException("I'm in your walls");
    }
}
