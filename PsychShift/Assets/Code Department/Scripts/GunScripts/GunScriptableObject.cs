using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    //Types of vars needed
    //public ImpactType ImpactType; IMPLEMENT AFTER WATCHING SURFACE TUTORIAL
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    //Refs to all the configs
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    private MonoBehaviour ActiveMonoBehavior;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;
    public void Spawn(Transform Parent, MonoBehaviour ActionMonoBehavior)
    {
        this.ActiveMonoBehavior = ActionMonoBehavior;
        LastShootTime = 0; //not reset in editior, fine in build for some reason.
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();

    }
    public void Shoot()
    {
        //Last shoot time matches with time.time when shooting// whenever fire rate is added it doesn't allow for shoot system to activate waits until time is greater to fire again
        if(Time.time > ShootConfig.FireRate +LastShootTime)
        {
            LastShootTime = Time.time;
            ShootSystem.Play();

            Vector3 shootDirection = ShootSystem.transform.forward + new Vector3(
                    Random.Range(-ShootConfig.Spread.x,ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y,ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
                );

            shootDirection.Normalize();

            if(Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
            {
                ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootSystem.transform.position,
                                                            hit.point,
                                                            hit)
                                                );
            }
            else
            {
                ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootSystem.transform.position, 
                                                            ShootSystem.transform.position + 
                                                            (shootDirection * TrailConfig.MissDistance),
                                                            new RaycastHit()
                                                            )
                                                );
            }
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint,Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance =  TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;//avoids position carry-over from last frame if resused

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while(remainingDistance >0)
        {
            instance.transform.position = Vector3.Lerp(StartPoint, EndPoint, Mathf.Clamp01(1- (remainingDistance/distance)));
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }
        instance.transform.position = EndPoint;
        /*if(Hit.collider != null)
        {
            SurfaceManager.instance.HandleImpact(Hit.transform.gameObject,EndPoint,Hit.normal, ImpactType, 0);
        }*/ //WATCH TUTORIAL ON IMPLEMENTING SURFACE MANAGER
        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;

    }

}
