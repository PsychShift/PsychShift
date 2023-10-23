using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    bool isActive= false;
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
    private void Update() 
    {
        /*if(isActive== true)
        {
            //bullet_rigidbody.velocity= 
        }*/
    }

    public void Spawn(Vector3 SpawnForce)
    {
        isActive = true;
        SpawnLocation = transform.position;
        transform.forward = SpawnForce.normalized;
        bullet_rigidbody.velocity = SpawnForce;
        //bullet_rigidbody.AddForce(SpawnForce);
        StartCoroutine(DelayedDisable(DelayedDisableTime));
    }

    private IEnumerator DelayedDisable(float Time)
    {
       yield return new WaitForSeconds(Time);
       isActive = false;
       bullet_rigidbody.velocity = Vector3.zero;
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
