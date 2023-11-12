using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShaderOnOff : MonoBehaviour
{
    public Camera postProcessingCamera; // Assign the camera with post-processing here
    public Camera noPostCam;
    private bool shaderOnOff;
    private void Update() {
        /*if(Input.GetMouseButtonDown(0))
            TurnOff();
        else if(Input.GetMouseButtonDown(1))
            TurnOn();*/
        if(InputManager1.Instance.PlayerSwitcherShader())
        {
            BoolOnOff();
        }

            
    }
    private void BoolOnOff()
    {
        if(shaderOnOff)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
        shaderOnOff=!shaderOnOff;
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

