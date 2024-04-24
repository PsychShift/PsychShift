using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDeathModifier : AbstractEnemyModifier
{
    EnemyBrain brain;
    AudioSource enemyVoice;
    public float radius = 30;
    public int damageAmount = 100;
    public float falloffRate =  0.2f;
    public override void ApplyModifier(EnemyBrain brain)
    {
        this.brain = brain;
        enemyVoice = GetComponent<AudioSource>();
        brain.onSwappedOut += HandleExplosion;
        StartCoroutine(WaitForEnemyHealth());
        StartCoroutine(VoiceLineSpam());
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

    private void HandleExplosion(Transform _)
    {   
        // Perform a sphere cast
        // play effect
        StartCoroutine(Instantiate(GameAssets.Instance.ExplosionEffect, transform.position, Quaternion.identity).GetComponent<ExplosionEffectScript>().Explode(4f, radius));

        // Define the falloff parameters
        float baseDamage = damageAmount; // The original damage amount

        // Handle damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider == null) continue;

            if(hitCollider.TryGetComponent(out IDamageable damageable))
            {
                // Calculate the distance from the explosion center to the hit collider
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                // Apply the falloff rate
                float falloffFactor = Mathf.Clamp01((radius - distance) / radius);
                float finalDamage = baseDamage * falloffFactor;

                // If the object has an IDamageable component, apply damage
                if (damageable != null)
                {
                    damageable.TakeDamage((int)finalDamage, Guns.GunType.None);
                }
            }
        }
    }
    
    IEnumerator VoiceLineSpam()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4,10));

            enemyVoice.PlayOneShot(AudioEnemy.AudioManager.Instance.bombEnemy[Random.Range(0, AudioEnemy.AudioManager.Instance.bombEnemy.Length)]);
            while(enemyVoice.isPlaying)
            {
                yield return new WaitForSeconds(4);
            }
        }        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
