using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]//Might have to turn this off for mindswapping. Guns changing on swap could bug cuz of this. Just a theory tho
public class GunHandler : MonoBehaviour
{
    public Camera Camera;

   [SerializeField]
   private Transform GunParent;
   [SerializeField]
   private GunScriptableObject SpawnGun;

   public int currentAmmo;
   /*[SerializeField]
   private PlayerIK InverseKinematics;*/ //No clue why this was in the tutorial. Maybe will b explained.
   [Space]
   [Header("Runtime Filled")]//don't know wut this means but tutorial said to put it there
   public GunScriptableObject ActiveGun;
   private void Start() 
   {
        if(SpawnGun == null)
        {
            Debug.LogError("No GunScriptableObject found for GunType: " + SpawnGun);
            return;
        }
        ActiveGun = SpawnGun;
        ActiveGun.Spawn(GunParent, this, Camera);
        currentAmmo = ActiveGun.AmmoConfig.CurrentClipAmmo;//Temp fix

        //Inverse kinematic stuff should go here but idk if we're doing all that
   }


    public void SetGun(GunScriptableObject gun) 
    {
        
        if(gun == null)
        {
            Debug.LogError($"No GunScriptableObjec found for GunType: {gun}");
            return;
        }
        if(ActiveGun !=null)
        {
            ActiveGun.DespawnGun();
        }
        ActiveGun = gun;
        gun.Spawn(GunParent, this, Camera);
        //currentBullets = gun.AmmoConfig.CurrentClipAmmo;//Temp fix

        //Inverse kinematic stuff should go here but idk if we're doing all that
    }

    public void EnemyShoot()//THIS IS CALLED WHENEVER THE ENEMY TRIES TO SHOOT
    {
        ActiveGun.TryToShoot(true);
    }

    public bool ShouldReload()
    {
        return currentAmmo <= 0;
    }

}
