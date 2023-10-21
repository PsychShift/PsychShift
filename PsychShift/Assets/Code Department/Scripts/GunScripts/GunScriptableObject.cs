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
    public DamageConfigScriptableObject DamageConfig;
    public AmmoConfigScriptableObject AmmoConfig;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    private MonoBehaviour ActiveMonoBehavior;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<Bullet> BulletPool;
    private ObjectPool<TrailRenderer> TrailPool;
    public void Spawn(Transform Parent, MonoBehaviour ActionMonoBehavior)
    {
        this.ActiveMonoBehavior = ActionMonoBehavior;
        LastShootTime = 0; //not reset in editior, fine in build for some reason.
        AmmoConfig.CurrentClipAmmo = AmmoConfig.ClipSize;
        AmmoConfig.CurrentAmmo = AmmoConfig.MaxAmmo;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        if(!ShootConfig.IsHitscan)
        {
            BulletPool = new ObjectPool<Bullet>(CreateBullet);
        }

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
            AmmoConfig.CurrentClipAmmo--; //TUTORIAL FOR RECOIL

            if(ShootConfig.IsHitscan)
            {
                DoHitscanShoot(shootDirection);
            }
            else
            {
                DoProjectileShoot(shootDirection);
            }

        }
    }

    public bool CanReload()
    {
        return AmmoConfig.CanReload();
    }
    /*
    public bool EndReload()
    {
        AmmoConfig.Reload();
    }*/
    
    public void Tick(bool WantsToShoot)//TUTORIAL FOR RECOIL
    {
        /*Model.transform.localRotation = Quaternion.Lerp(
            Model.transform.localRotation,
            Quaternion.Euler(SpawnRotation),
            Time.deltaTime * ShootConfig.RecoilRecoverSpeed
        );*/

        if(WantsToShoot)
        {
            //LastFrameWantedToShoot = true;
            if(AmmoConfig.CurrentClipAmmo>0)
            {
                Shoot();
            }
        }

        /*if(!WantsToShoot && LastFrameWantedToShoot)
        {
            StopShootingTime = Time.time;
            LastFrameWantedToShoot = false;
        }*/
    }
    private void DoHitscanShoot(Vector3 ShootDirection)
    {
            if(Physics.Raycast(ShootSystem.transform.position, ShootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
            {
                ActiveMonoBehavior.StartCoroutine(PlayTrail
                (ShootSystem.transform.position,hit.point,hit));
            }
            else
            {
                ActiveMonoBehavior.StartCoroutine
                    (PlayTrail(ShootSystem.transform.position, 
                     ShootSystem.transform.position + 
                     (ShootDirection * TrailConfig.MissDistance),
                     new RaycastHit()
                     )
                );
            }
    }

    private void DoProjectileShoot(Vector3 ShootDirection)
    {
       Bullet bullet = BulletPool.Get();
       bullet.gameObject.SetActive(true);
       bullet.OnCollision += HandleBulletCollision;
       bullet.transform.position = ShootSystem.transform.position;
       bullet.Spawn(ShootDirection * ShootConfig.BulletSpawnForce);

       TrailRenderer trail = TrailPool.Get();
       if(trail != null)
       {
            trail.transform.SetParent(bullet.transform, false);
            trail.transform.localPosition = Vector3.zero;
            trail.emitting = true;
            trail.gameObject.SetActive(true);
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
        if(Hit.collider != null)
        {
            HandleBulletImpact(distance, EndPoint, Hit.normal, Hit.collider);
        } //WATCH TUTORIAL ON IMPLEMENTING SURFACE MANAGER
        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private void HandleBulletCollision(Bullet Bullet, Collision Collision)
    {
        TrailRenderer trail = Bullet.GetComponentInChildren<TrailRenderer>();
        if(trail != null)
        {
            trail.transform.SetParent(null, true);
            ActiveMonoBehavior.StartCoroutine(DelayedDisableTrail(trail));
        }

        Bullet.gameObject.SetActive(false);
        BulletPool.Release(Bullet);
        if(Collision != null)
        {
           ContactPoint contactPoint = Collision.GetContact(0);

           HandleBulletImpact(
                Vector3.Distance(contactPoint.point, Bullet.SpawnLocation),
                contactPoint.point,
                contactPoint.normal,
                contactPoint.otherCollider);
        }
    }

    private void HandleBulletImpact(
        float DistanceTraveled,
        Vector3 HitLocation,
        Vector3 HitNormal,
        Collider HitCollider)
    {
        //rip da dream
        /*SurfaceManager.Instance.HandleImpact(
            HitCollider.gameObject,
            HitLocation,
            HitNormal,
            ImpactType,
            0

        );*/

        if(HitCollider.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(DamageConfig.GetDamage(DistanceTraveled));
        }
    }

    private IEnumerator DelayedDisableTrail(TrailRenderer Trail)
    {
        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        Trail.emitting = false;
        Trail.gameObject.SetActive(false);
        TrailPool.Release(Trail);
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

    private Bullet CreateBullet()
    {
        return Instantiate(ShootConfig.BullerPrefab);
    }

}
