using System.Collections;
using System.Collections.Generic;
using Guns.Health;
using UnityEngine;

public class TeslaBeam : MonoBehaviour
{
    public LaserBeamStats defaultStats;
    public float cooldownTimer = 10f;
    [SerializeField] Vector2Int maxNumOfTargets = new Vector2Int(3, 6);
    [SerializeField] float laserSpeed = 10f;

    [SerializeField] float targetCheckRadius = 1000;
    LineRenderer lineRenderer;
    public bool test;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(test)
            ShootLaserInstant();
        else
            StartCoroutine(ShootLaser(defaultStats));
    }
    public List<Vector3> GetTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, targetCheckRadius);
        if (hits.Length == 0) return null;
        List<Vector3> positions = new ();
        int max = Random.Range(maxNumOfTargets.x, maxNumOfTargets.y+1);
        int numOfTargets = hits.Length > max ? max : hits.Length;
        int targetsAdded =  0; // Keep track of how many targets have been added

        for(int i =  0; i < hits.Length; i++)
        {
            if (targetsAdded >= numOfTargets) break; // Stop if max targets reached

            if (hits[i].transform.TryGetComponent(out IDamageable damageable))
            {
                if(damageable is ChildCollider)
                {
                    continue; // Skip ChildCollider
                }  
                Vector3 pos = hits[i].transform.position;
                if(damageable is EnemyHealth) pos.y +=  4.5f;
                positions.Add(pos);
                targetsAdded++; // Increment the count of targets added
            }
        }
        positions.Sort(new RadialSorter(transform.position));

        return positions;
    }

    public void ShootLaserInstant()
    {
        lineRenderer.enabled = true;
        List<Vector3> positions = GetTargets();
        positions.Insert(0, transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    IEnumerator ShootLaser(LaserBeamStats currentStats)
    {
        List<Vector3> positions = GetTargets();
        Vector3 currentPos = transform.position;
        positions.Insert(0, currentPos);
        lineRenderer.positionCount =  2;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, currentPos);
        bool hitPlayer = false;
        float hitPlayerAgainTimer = 0f;


        for (int i =  1; i < positions.Count; i++)
        {
            if(hitPlayer)
            {
                if(Time.time > hitPlayerAgainTimer) hitPlayer = false;
            }
            
            Vector3 startPos = currentPos;
            Vector3 targetPos = positions[i];
            float distance = Vector3.Distance(startPos, targetPos);
            float moveDir = laserSpeed * Time.deltaTime;

            while (moveDir < distance)
            {
                currentPos = Vector3.Lerp(startPos, targetPos, moveDir / distance);
                moveDir += laserSpeed * Time.deltaTime;
                lineRenderer.SetPosition(i, currentPos);
                for(int j = 0; j < i; j++)
                {
                    Vector3 startPosition = positions[j];
                    Vector3 endPosition = j != i - 1 ? positions[j +  1] : currentPos;

                    // Calculate the direction from the start to the end position
                    Vector3 direction = (endPosition - startPosition).normalized;

                    // Calculate the distance between the two positions
                    float maxDistance = Vector3.Distance(startPosition, endPosition);

                    // Perform the SphereCastAll from the start position to the end position
                    RaycastHit[] hits = Physics.SphereCastAll(startPosition, currentStats.Width, direction, maxDistance);

                    // Process the results of the SphereCastAll
                    foreach (RaycastHit hit in hits)
                    {
                        if(hit.transform.TryGetComponent(out IDamageable damageable))
                        {
                            // So the player doesn't get hit every frame
                            if(hit.transform.tag == "Player" && !hitPlayer)
                            {
                                hitPlayer = true;
                                hitPlayerAgainTimer = Time.time + currentStats.DamageSpeed;
                            }
                            else if(hit.transform.tag == "Player" && hitPlayer) continue; // If the player was hit recently, skip it to not do damage
                            damageable.TakeDamage((int)currentStats.Damage);
                        }
                    }
                }

                yield return null;
            }
            

            // Ensure the final position is exactly the target position
            currentPos = targetPos;
            lineRenderer.SetPosition(i, currentPos);
            if (i < positions.Count -  1)
                lineRenderer.positionCount++;
        }
        float duration = currentStats.ShootForTime;

        float endTime = Time.time + duration;

        while(Time.time < endTime)
        {
            if(hitPlayer)
            {
                if(Time.time > hitPlayerAgainTimer) hitPlayer = false;
            }
            for(int i = 0; i < positions.Count - 1; i++)
            {
                Vector3 startPosition = positions[i];
                Vector3 endPosition = positions[i +  1];

                // Calculate the direction from the start to the end position
                Vector3 direction = (endPosition - startPosition).normalized;

                // Calculate the distance between the two positions
                float maxDistance = Vector3.Distance(startPosition, endPosition);

                // Perform the SphereCastAll from the start position to the end position
                RaycastHit[] hits = Physics.SphereCastAll(startPosition, currentStats.Width, direction, maxDistance);

                // Process the results of the SphereCastAll
                foreach (RaycastHit hit in hits)
                {
                    if(hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        // So the player doesn't get hit every frame
                        if(hit.transform.tag == "Player" && !hitPlayer)
                        {
                            hitPlayer = true;
                            hitPlayerAgainTimer = Time.time + currentStats.DamageSpeed;
                        }
                        else if(hit.transform.tag == "Player" && hitPlayer) continue; // If the player was hit recently, skip it to not do damage
                        damageable.TakeDamage((int)currentStats.Damage);
                    }
                }
            }
            yield return null;
        }
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(cooldownTimer);
        StartCoroutine(ShootLaser(defaultStats));
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetCheckRadius);
    }
    #endif
}
