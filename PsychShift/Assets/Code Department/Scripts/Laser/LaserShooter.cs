using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserShooter : MonoBehaviour
{
    [SerializeField] private bool isOnTimer = false;
    public LaserBeamStats defaultStats;

    private bool isActivelyShooting = false;


    LineRenderer laserLine;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        laserLine.positionCount = 2;
        Vector3 pos = transform.position + transform.forward * defaultStats.MaxLength/2f;
        
        if(isOnTimer)
        {
            Fire();
        }
    }

    /// <summary>
    /// If endless == true, the laser will not turn off unless the CeaseFire method is called.
    /// </summary>
    public void Fire(bool endless = false, LaserBeamStats stats = null)
    {
        if(isActivelyShooting) 
        {
            StopAllCoroutines();
            isActivelyShooting = false;
        }
        if(stats == null) stats = defaultStats;
        if(endless)
            StartCoroutine(FireCoroutineEndless(stats));
        else
            StartCoroutine(FireCoroutine(stats));
    }
    public void CeaseFire()
    {
        StopAllCoroutines();
        laserLine.enabled = false;
        isActivelyShooting = false;
    }
    private IEnumerator FireCoroutine(LaserBeamStats currentStats)
    {
        float duration = currentStats.ShootForTime;

        isActivelyShooting = true;
        float endTime = Time.time + duration;
        bool hitPlayer = false;
        float hitPlayerAgainTimer = 0f;
        //cylinder.SetActive(true);
        laserLine.enabled = true;
        while(Time.time < endTime)
        {
            // If the player was hit, and the timer is over, set hitPlayer to false
            if(hitPlayer)
            {
                if(Time.time > hitPlayerAgainTimer) hitPlayer = false;
            }
            // Fire a spherecast all
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit endPointHit, currentStats.MaxLength);
            Vector3 endPoint = endPointHit.transform != null ? endPointHit.point : transform.position + transform.forward * currentStats.MaxLength;
            RaycastHit[] hits = Physics.SphereCastAll(ray, currentStats.Width, currentStats.MaxLength);

            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, endPoint);


            foreach(var hit in hits)
            {
                if(hit.transform.TryGetComponent(out IDamageable hitObject))
                {
                    // So the player doesn't get hit every frame
                    if(hit.transform.tag == "Player" && !hitPlayer)
                    {
                        hitPlayer = true;
                        hitPlayerAgainTimer = Time.time + currentStats.DamageSpeed;
                    }
                    else if(hit.transform.tag == "Player" && hitPlayer) continue; // If the player was hit recently, skip it to not do damage
                    hitObject.TakeDamage((int)currentStats.Damage);
                }
            }
            yield return null;
        }
        //cylinder.SetActive(false);
        laserLine.enabled = false;

        yield return new WaitForSeconds(currentStats.RestForTime);
        StartCoroutine(FireCoroutine(currentStats));
    }

    private IEnumerator FireCoroutineEndless(LaserBeamStats currentStats)
    {
        isActivelyShooting = true;
        bool hitPlayer = false;
        float hitPlayerAgainTimer = 0f;
        //cylinder.SetActive(true);
        laserLine.enabled = true;
        while(true)
        {
            // If the player was hit, and the timer is over, set hitPlayer to false
            if(hitPlayer)
            {
                if(Time.time > hitPlayerAgainTimer) hitPlayer = false;
            }
            // Fire a spherecast all
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit endPointHit, currentStats.MaxLength);
            Vector3 endPoint = endPointHit.transform != null ? endPointHit.point : transform.position + transform.forward * currentStats.MaxLength;
            RaycastHit[] hits = Physics.SphereCastAll(ray, currentStats.Width, currentStats.MaxLength);

            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, endPoint);


            foreach(var hit in hits)
            {
                if(hit.transform.TryGetComponent(out IDamageable hitObject))
                {
                    // So the player doesn't get hit every frame
                    if(hit.transform.tag == "Player" && !hitPlayer)
                    {
                        hitPlayer = true;
                        hitPlayerAgainTimer = Time.time + currentStats.DamageSpeed;
                    }
                    else if(hit.transform.tag == "Player" && hitPlayer) continue; // If the player was hit recently, skip it to not do damage
                    hitObject.TakeDamage((int)currentStats.Damage);
                }
            }
            yield return null;
        }
    }


    void OnDrawGizmos()
    {
        if(defaultStats == null) return;
        
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.color =  Physics.Raycast(ray, out RaycastHit hit, defaultStats.MaxLength) ? Color.green : Color.red;
        Vector3 endPoint = hit.transform != null ? hit.point : transform.position + transform.forward * defaultStats.MaxLength;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
