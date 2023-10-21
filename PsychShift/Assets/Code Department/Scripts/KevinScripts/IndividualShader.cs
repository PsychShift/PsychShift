using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class IndividualShader : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start() 
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Shader.EnableKeyword("Simple_Object_Black");
            Shader.EnableKeyword("Simple_Object_Blue");
            Shader.EnableKeyword("Simple_Object_Black");
            Shader.EnableKeyword("Simple_Gun 1");
        }
        else if(Input.GetMouseButton(1))
        {
            Shader.DisableKeyword("Simple_Object_Black");
            Shader.DisableKeyword("Simple_Object_Blue");
            Shader.DisableKeyword("Simple_Object_Black");
            Shader.DisableKeyword("Simple_Gun 1");
        }

    }
}
