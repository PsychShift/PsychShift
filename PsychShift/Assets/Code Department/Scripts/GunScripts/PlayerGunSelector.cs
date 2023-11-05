using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]//Might have to turn this off for mindswapping. Guns changing on swap could bug cuz of this. Just a theory tho
public class PlayerGunSelector : MonoBehaviour
{
    public Camera Camera;
   [SerializeField]
   private GunType Gun; 
   [SerializeField]
   private Transform GunParent;
   [SerializeField]
   private List<GunScriptableObject> Guns;

   public int currentBullets;
   /*[SerializeField]
   private PlayerIK InverseKinematics;*/ //No clue why this was in the tutorial. Maybe will b explained.
   [Space]
   [Header("Runtime Filled")]//don't know wut this means but tutorial said to put it there
   public GunScriptableObject ActiveGun;
   private void Start() 
   {
        GunScriptableObject gun= Guns.Find(gun => gun.Type == Gun);

        if(gun == null)
        {
            Debug.LogError($"No GunScriptableObjec found for GunType: {gun}");
            return;
        }
        ActiveGun = gun;
        gun.Spawn(GunParent, this, Camera);
        //currentBullets = gun.AmmoConfig.CurrentClipAmmo;//Temp fix

        //Inverse kinematic stuff should go here but idk if we're doing all that


   }


}
