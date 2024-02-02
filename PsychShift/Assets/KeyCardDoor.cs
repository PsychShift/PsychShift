using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenDaNoor()
    {
        this.GetComponent<Collider>().enabled = false;
        
    }
}
