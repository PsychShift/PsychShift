using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public EnemyHealth Health;
    //public EnemyMovement Movement;
    //public EnemyPainResponse PainResponse;
    //useless script rn L tutorial 

    private void Start()
    {
        //Health.OnTakeDamage += PainResponse.HandlePain;
        Health.OnDeath += Die;//here goes the death stuff// Talks to enemy script and goes to interface
    }

    private void Die(Transform Position)//death stuff is given to health funct in enemyhealth script
    {
        //Movement.StopMoving();
        //PainResponse.HandleDeath();
    }
}
