using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class ModeSwitch : MonoBehaviour
{
    PlayerStateMachine1 playerStateMachine;
    public bool canHang = false;
    void Update()
    {
        if(InputManager1.Instance.PlayerSwitchedModeThisFrame())
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
