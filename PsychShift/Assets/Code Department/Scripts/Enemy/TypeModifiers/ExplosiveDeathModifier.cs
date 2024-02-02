using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDeathModifier : AbstractEnemyModifier
{
    EnemyBrain brain;
    float radius = 10;
    int damageAmount = 50;
    public override void ApplyModifier(EnemyBrain brain)
    {
        this.brain = brain;
        StartCoroutine(WaitForEnemyHealth());
    }
    IEnumerator WaitForEnemyHealth()
    {
        while(brain.EnemyHealth == null)
        {
            yield return new WaitForEndOfFrame();
        }
        brain.EnemyHealth.OnDeath += HandleDeath;
    }

    private void HandleDeath(Transform idk)
    {   
        Debug.Log("explode");
        // Perform a sphere cast
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();

            // If the object has an IDamageable component, apply damage
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
        }
    }
    /* void OnDestroy()
    {
        brain.EnemyHealth.OnDeath -= HandleDeath;
    } */
}
