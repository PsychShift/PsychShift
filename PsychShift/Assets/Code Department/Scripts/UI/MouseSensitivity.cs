using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider sensitivityBar;
    public bool initalized = false;
    void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivityBar.value = PlayerPrefs.GetFloat ("Sensitivity");
        }
        initalized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMouseSensitivity(float val)
    {
        if(! initalized) return;
        if(! Application.isPlaying) return;
        
        PlayerPrefs.SetFloat("Sensitivity", val);

    }
}
