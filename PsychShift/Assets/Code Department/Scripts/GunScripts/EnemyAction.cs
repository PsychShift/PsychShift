using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class EnemyShootScript : MonoBehaviour
{
    [SerializeField]
    private GunHandler1 GunSelector;
    [SerializeField]
    private bool AutoReload = true;

    //private float ReloadSpeed = 1f;
    /* ANIMATION FOR RELOAD
    [SerializeField]
    private Animator PlayerAnimator;
    [SerializeField]
    private PlayerIK InverseKinematics;
    private bool IsReloading;
    */
}
