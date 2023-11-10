using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private GunHandler GunSelector;
    [SerializeField]
    private bool AutoReload = true;
    [SerializeField]
    private Image Crosshair;
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
        //GunSelector.ActiveGun.TryToShoot();
        GunSelector.ActiveGun.Tick(GunSelector.ActiveGun != null);
    }
    //private void FixedUpdate() 
    //{
         /*if(InputManager.Instance.ShootAction.triggered && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.TryToShoot();
        }*/ // Prolly add this to the main player script

        //UpdateCrosshair();

        //GunSelector.ActiveGun.Tick(InputManager.Instance.PlayerShotThisFrame() && GunSelector.ActiveGun != null);//DO DIS AFTER RECOIL TUTORIAL REPLACE UP

        /*if(ShouldManualReload() || ShouldAutoReload())
        {
            GunSelector.ActiveGun.StartReloading();
            IsReloading = true;
            PlayerAnimator.SetTrigger("Reload");
            InverseKinemcatics.HandIKAmount = 0.25;
            InverseKinematics.ElbowIKAmount = 0.25f;
        }*/
    //}
    private void UpdateCrosshair()
    {
        if(GunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
        {
            Vector3 gunTipPoint = GunSelector.ActiveGun.GetRaycastOrigin();
            Vector3 gunForward = GunSelector.ActiveGun.GetGunForward();
            Vector3 hitPoint = gunTipPoint + gunForward * 10;
            if(Physics.Raycast(
                gunTipPoint,
                gunForward,
                out RaycastHit hit,
                float.MaxValue,
                GunSelector.ActiveGun.ShootConfig.HitMask
            ))
            {
                hitPoint = hit.point;
            }

            Vector3 screenSpaceLocation = GunSelector.Camera.WorldToScreenPoint(hitPoint);
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)Crosshair.transform.parent, screenSpaceLocation, null,
            out Vector2 localPosition))
            {
                Crosshair.rectTransform.anchoredPosition = localPosition;
            }
            else
            {
                Crosshair.rectTransform.anchoredPosition = Vector2.zero;  
            }
            
        }
    }
    
    /*private void EndReload()
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
