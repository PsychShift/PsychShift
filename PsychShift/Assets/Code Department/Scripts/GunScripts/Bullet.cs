using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody bullet_rigidbody;
    [field: SerializeField]

    public Vector3 SpawnLocation
    {
        get;
        private set;
    }
    [SerializeField]
    private float DelayedDisableTime = 2f;

    public delegate void CollisionEvent(Bullet Bullet, Collision collision);
    public event CollisionEvent OnCollision;

    private void Awake() 
    {
        bullet_rigidbody = GetComponent<Rigidbody>();   
    }

    public void Spawn(Vector3 SpawnForce)
    {
        SpawnLocation = transform.position;
        transform.forward = SpawnForce.normalized;
        bullet_rigidbody.AddForce(SpawnForce);
        StartCoroutine(DelayedDisable(DelayedDisableTime));
    }

    private IEnumerator DelayedDisable(float Time)
    {
       yield return new WaitForSeconds(Time);
       OnCollisionEnter(null);

    }
    private void OnCollisionEnter(Collision collision) 
    {
        OnCollision?.Invoke(this, collision);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        bullet_rigidbody.velocity = Vector3.zero;
        bullet_rigidbody.angularVelocity = Vector3.zero;
        OnCollision = null;

    }

}
