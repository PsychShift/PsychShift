using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingTrap : MonoBehaviour, IManipulate
{
    public float damageModifier = 1f;
    public bool IsInteracted { get; set; }
    public bool CanInteract { get; set; }

    private Rigidbody rb;

    void Start()
    {
        Debug.Log(GetUnlitShader());
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    static string GetUnlitShader()
        {
#if USING_URP
            return "Universal Render Pipeline/Unlit";
#elif USING_HDRP
            return "HDRP/Unlit";
#else
            return "Unlit/Color";
#endif
        }
    public void Interact()
    {
        rb.useGravity = true;
        Debug.Log(rb.useGravity);
    }

    public void Interacted()
    {
        
        rb.useGravity = true;
        Debug.Log(rb.useGravity);
    }

    public void ReverseInteract()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.TryGetComponent(out IDamageable damageable))
        {
            float damage = rb.velocity.magnitude * damageModifier;
            damageable.TakeDamage((int)damage);
        }
    }
}
