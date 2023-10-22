using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    [SerializeField]
    private bool AutoReload = true;
    public int currentBullets;
    //private float ReloadSpeed = 1f;
    /* ANIMATION FOR RELOAD
    [SerializeField]
    private Animator PlayerAnimator;
    [SerializeField]
    private PlayerIK InverseKinematics;
    private bool IsReloading;
    */

    private void OnEnable() 
    {
        InputManager.Instance.OnShootPressed += Shoot;
        //currentBullets =GunSelector.currentBullets;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnShootPressed -= Shoot;
    }
    private void Shoot()
    {
        GunSelector.ActiveGun.Tick(GunSelector.ActiveGun != null);
    }
    private void FixedUpdate() 
    {
        /*if(InputManager.Instance.PlayerShotThisFrame() && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Shoot();
        }*/ // Prolly add this to the main player script

        //GunSelector.ActiveGun.Tick(InputManager.Instance.PlayerShotThisFrame() && GunSelector.ActiveGun != null);//DO DIS AFTER RECOIL TUTORIAL REPLACE UP

        /*if(ShouldManualReload() || ShouldAutoReload())
        {
            IsReloading = true;
            PlayerAnimator.SetTrigger("Reload");
            InverseKinemcatics.HandIKAmount = 0.25;
            InverseKinematics.ElbowIKAmount = 0.25f;
        }*/
    }
    /*
    private void EndReload()
    {
       GunSelector.ActiveGun.EndReload();
       InverseKinemcatics.HandIKAmount = 1;
       InverseKinematics.ElbowIKAmount = 1;
       IsReloading = false; 
    }*/

    /*private bool ShouldManualReload()
    {
        return Keyboard.current.rKey.wasReleasedThisFrame 
        && GunSelector.ActiveGun.CanReload();
    }

    private bool ShouldAutoReload()
    {
        return AutoReload
                && GunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo ==0
                && GunSelector.ActiveGun.CanReload();
    }*/

    
}
