using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
            StaticBar.instance.UseStatic(15);
    }
}
