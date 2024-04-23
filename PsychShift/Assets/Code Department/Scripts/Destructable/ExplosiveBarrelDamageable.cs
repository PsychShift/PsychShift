using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;

public class ExplosiveBarrelDamageable : MonoBehaviour, IDamageable
{
    #region Not Useful, waaa
    public float CurrentHealth { get; }

    public float MaxHealth { get; }

    public bool IsWeakPoint { get; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #endregion

    public float radius = 10;
    public int damageAmount = 50;
    public float falloffRate =  0.2f;
    [SerializeField] private Transform explosiveCenterRadius;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosion;
    [SerializeField] private ExplosionEffectScript explosionEffect;

    public void TakeDamage(float Damage, GunType gunType)
    {
        Debug.Log("Kevin is Stinky, Explosive Barrel 1");
        GetComponent<BoxCollider>().enabled = false;
        model.SetActive(false);
        explosion.SetActive(true);
        StartCoroutine(explosionEffect.Explode(-1, radius));
        HandleExplosion();

        StartCoroutine(DestroyAfterDelay(5f));
    }

    private void HandleExplosion()
    {   
        // Perform a sphere cast
        // play effect

        // Define the falloff parameters
        float baseDamage = damageAmount; // The original damage amount

        // Handle damage
        Collider[] hitColliders = Physics.OverlapSphere(explosiveCenterRadius.position, radius);

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
                    damageable.TakeDamage((int)finalDamage, GunType.None);
                }
            }
        }
    }

    private IEnumerator DestroyAfterDelay(float delayInSeconds)
    {
        //Unbake navmesh here
        yield return new WaitForSeconds(delayInSeconds);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
