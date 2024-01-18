using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyWall : MonoBehaviour

{
    public int damage = 10; // Set the damage value for the bullet

    void OnCollisionEnter(Collision collision)
    {
        DestructableDamageable destructable = collision.gameObject.GetComponent<DestructableDamageable>();

        // Check if the collided object has the DestructableDamageable script
        if (destructable != null)
        {
            // Apply damage to the object
            destructable.TakeDamage(damage);

            // Destroy the bullet
            Destroy(gameObject);

            Debug.Log("Bullet hit and damaged the wall!");
        }
        else
        {
            Debug.Log("Bullet hit something, but it's not damageable.");
        }
    }
}