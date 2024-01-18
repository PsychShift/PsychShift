using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propwalldestory : MonoBehaviour

{
    public int damageAmount = 10; // Set the damage amount

    void OnTriggerEnter(Collider other)
    {
        DestructableDamageable destructable = other.GetComponent<DestructableDamageable>();

        // Check if the collided object has the DestructableDamageable script
        if (destructable != null)
        {
            // Apply damage to the object
            destructable.TakeDamage(damageAmount);
        }
    }
}