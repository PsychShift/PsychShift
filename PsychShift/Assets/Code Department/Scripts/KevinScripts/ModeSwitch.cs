using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class ModeSwitch : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    public bool canHang = false;
    void Update()
    {
        if(InputManager.Instance.PlayerSwitchedModeThisFrame())
        {
            if(canHang == false)
            {
                canHang = true;
            }
            else if(canHang == true)
            {
                canHang = false;
            }
        }
    }
}
