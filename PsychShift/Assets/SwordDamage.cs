using System.Collections;
using System.Collections.Generic;
using Guns.Health;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    // Start is called before the first frame update
    private bool damageFlag;
    public float swordDmg;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer!=15 && damageFlag == false)
        {
            //do damage once then reset flag
                //do damage
                other.GetComponent<EnemyHealth>().TakeDamage(swordDmg,Guns.GunType.None);
                damageFlag = true;
                StartCoroutine(swordDownTime());
            

        }    
    }
    IEnumerator swordDownTime()
    {
        yield return new WaitForSeconds(1);
        damageFlag = false;
    }
}
