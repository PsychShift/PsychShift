using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    [HideInInspector] public Transform gauranteedPlayer => EnemyTargetManager.Instance.player;
    private GameObject _player;
    public GameObject playerRef 
    { 
        get 
        { 
            if(_player == null)
                return gauranteedPlayer.gameObject; 
            else
                return _player.gameObject;
        } 
        set { _player = value.gameObject; } 
    }

    private void Start() 
    {
         
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        float delay = .2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if(rangeChecks.Length !=0)
        {
           Transform target = rangeChecks[0].transform;
           Vector3 directionToTarget = (target.position- transform.position).normalized;

           if(Vector3.Angle(transform.forward, directionToTarget)< angle/2)
           {
              float distanceToTarget = Vector3.Distance(transform.position, target.position);

              if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
              {
                    canSeePlayer = true;
              }
              else
              {
                    canSeePlayer = false;
              }
           }
           else
           {
                canSeePlayer = false;
           } 
        }
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
