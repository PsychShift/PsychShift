using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    public GameObject Phase1;
    public GameObject Phase2;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            Phase1.SetActive(false);
            Phase2.SetActive(true);
        }
    }
}
