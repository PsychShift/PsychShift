using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public GameObject CreditsObject;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        CreditsObject.SetActive(true);
        AudioListener.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //CreditsObject.SetActive(true);
    }
}
