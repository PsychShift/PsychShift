using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ShaderOnOff : MonoBehaviour
{
    public Camera postProcessingCamera; // Assign the camera with post-processing here
    public Camera noPostCam;
    private void Update() {
        if(Input.GetMouseButtonDown(0)|| Input.GetButtonDown("OnShader"))
            TurnOff();
        else if(Input.GetMouseButtonDown(1)|| Input.GetButtonDown("OffShader"))
            TurnOn();
            
    }

    private void TurnOff()
    {
        postProcessingCamera.enabled = false;
        noPostCam.enabled = true;
    }
    private void TurnOn()
    {
        postProcessingCamera.enabled = true;
        noPostCam.enabled = false;
    }


}

