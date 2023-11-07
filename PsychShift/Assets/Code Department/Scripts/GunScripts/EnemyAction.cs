using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class EnemyAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    [SerializeField]
    private bool AutoReload = true;
    [SerializeField]
    //private float ReloadSpeed = 1f;
    /* ANIMATION FOR RELOAD
    [SerializeField]
    private Animator PlayerAnimator;
    [SerializeField]
    private PlayerIK InverseKinematics;
    private bool IsReloading;
    */
    public void EnemyShoot()//THIS IS CALLED WHENEVER THE ENEMY TRIES TO SHOOT
    {
        GunSelector.ActiveGun.TryToShoot(true);
    }
}
