using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerAction : MonoBehaviour
    {
        // public for editor
        public PlayerGunSelector GunSelector;
        [SerializeField]
        private bool AutoReload = false;
        [SerializeField]
        private PlayerIK InverseKinematics;
        [SerializeField]
        private Animator PlayerAnimator;
        [SerializeField]
        private bool IsReloading;

        private void Update()
        {
            if(GunSelector.ActiveGun == null) return;
            
            GunSelector.ActiveGun.Tick(
                !IsReloading
                && Application.isFocused && InputManager.Instance.IsShootingHeld
                && GunSelector.ActiveGun != null
            );

            if (ShouldManualReload() || ShouldAutoReload())
            {
                GunSelector.ActiveGun.StartReloading();
                IsReloading = true;
                EndReload();
                /*PlayerAnimator.SetTrigger("Reload");
                InverseKinematics.HandIKAmount = 0.25f;
                InverseKinematics.ElbowIKAmount = 0.25f; */
            }
        }

        private bool ShouldManualReload()
        {
            return !IsReloading
                && Keyboard.current.rKey.wasReleasedThisFrame
                && GunSelector.ActiveGun.CanReload();
        }

        private bool ShouldAutoReload()
        {
            return !IsReloading
                && AutoReload
                && GunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo == 0
                && GunSelector.ActiveGun.CanReload();
        }

        private void EndReload()
        {
            GunSelector.ActiveGun.EndReload();
            /* InverseKinematics.HandIKAmount = 1f;
            InverseKinematics.ElbowIKAmount = 1f; */
            IsReloading = false;
        }
    }
}