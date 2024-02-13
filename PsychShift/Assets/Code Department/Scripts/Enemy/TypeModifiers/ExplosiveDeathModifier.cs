using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDeathModifier : AbstractEnemyModifier
{
    EnemyBrain brain;
    public float radius = 100;
    public int damageAmount = 50;
    public float falloffRate =  0.2f;
    public override void ApplyModifier(EnemyBrain brain)
    {
        this.brain = brain;
        brain.onSwappedOut += HandleExplosion;
        StartCoroutine(WaitForEnemyHealth());
    }
    void OnDestroy()
    {
        brain.onSwappedOut -= HandleExplosion;
    }
    IEnumerator WaitForEnemyHealth()
    {
        while(brain.EnemyHealth == null)
        {
            yield return new WaitForEndOfFrame();
        }
        brain.EnemyHealth.OnDeath += HandleExplosion;
    }

    private void HandleExplosion(Transform idk)
    {   
        // Perform a sphere cast
        // play effect

        // Define the falloff parameters
        float baseDamage = damageAmount; // The original damage amount

        // Handle damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();

            // Calculate the distance from the explosion center to the hit collider
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

            // Apply the falloff rate
            float falloffFactor = Mathf.Clamp01((radius - distance) / radius);
            float finalDamage = baseDamage * falloffFactor;

            // If the object has an IDamageable component, apply damage
            if (damageable != null)
            {
                damageable.TakeDamage((int)finalDamage);
            }
        }
    }
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
