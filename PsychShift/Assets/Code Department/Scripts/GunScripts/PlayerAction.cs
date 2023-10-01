using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    private void Update() 
    {
        if(InputManager.Instance.PlayerShotThisFrame() && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Shoot();
        } // Prolly add this to the main player script
    }

    
}
