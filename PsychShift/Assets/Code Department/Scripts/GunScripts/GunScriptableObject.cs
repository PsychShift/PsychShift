using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    public AudioConfigScriptableObject AudioConfig;


    private MonoBehaviour ActiveMonoBehavior;
    private GameObject Model;
    private Camera ActiveCamera; 
    private AudioSource ShootingAudioSource;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<Bullet> BulletPool;
    private ObjectPool<TrailRenderer> TrailPool;
    public void Spawn(Transform Parent, MonoBehaviour ActionMonoBehavior, Camera ActiveCamera = null)
    {
        this.ActiveMonoBehavior = ActionMonoBehavior;
        this.ActiveCamera =  ActiveCamera;
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
        ShootingAudioSource = Model.GetComponent<AudioSource>();//gets weapon audio stuff   
    } 
    public void DespawnGun()
    {
        Destroy(Model);
        //throw new System.NotImplementedException();
    }

    public void UpdateCamera(Camera ActivateCamera)
    {
        this.ActiveCamera = ActivateCamera;
    }
    public void TryToShoot(bool isEnemy)//Calls for bool to detect if enemy
    {
        //Last shoot time matches with time.time when shooting// whenever fire rate is added it doesn't allow for shoot system to activate waits until time is greater to fire again
        /*if(Time.time - LastShootTime - ShootConfig.FireRate > Time.deltaTime)
        {
            float lastDuration = Mathf.Clamp(
                (StopShootingTime- InitialClickTime)
            );
            float lerpTime = (ShootConfig.RecoilRecoverySpeed - (Time.time - StopShootingTime)) / ShootConfig.RecoilRecoverySpeed;

            InitialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
        }*/
        if(Time.time > ShootConfig.FireRate +LastShootTime)
        {

            LastShootTime = Time.time;
            if(AmmoConfig.CurrentClipAmmo ==0)
            {
                AudioConfig.PlayOutOfAmmoClip(ShootingAudioSource);
                return;
            }
            //Play shoot animation spin ball fast//get component for animator//mess with var here to ramp up//swap back when no shoot
            ShootSystem.Play();
            AudioConfig.PlayShootingClip(ShootingAudioSource, AmmoConfig.CurrentClipAmmo == 1);

            Vector3 shootDirection = ShootSystem.transform.forward + new Vector3(
                    Random.Range(-ShootConfig.Spread.x,ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y,ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
                );
            if(ShootConfig.ShootType == ShootType.FromGun)
            {
                shootDirection = ShootSystem.transform.forward + new Vector3(
                    Random.Range(-ShootConfig.Spread.x,ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y,ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
                );
            }
            else
            {
                shootDirection = ActiveCamera.transform.forward + ActiveCamera.transform.TransformDirection(shootDirection);
            }

            shootDirection.Normalize();
            if(isEnemy == true)
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
    public void StartReloading()//Reload sound fx
    {
        AudioConfig.PlayReloadClip(ShootingAudioSource);
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
            TryToShoot(false);//Calls false cuz player calls from tick

        }

        /*if(!WantsToShoot && LastFrameWantedToShoot)
        {
            StopShootingTime = Time.time;
            LastFrameWantedToShoot = false;
        }*/
    }
    private void DoHitscanShoot(Vector3 ShootDirection)
    {
            if(Physics.Raycast(GetRaycastOrigin(), ShootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
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

       if(ShootConfig.ShootType ==  ShootType.FromCamera && Physics.Raycast(GetRaycastOrigin(),ShootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
       {
            Vector3 directionToHit = (hit.point - ShootSystem.transform.position).normalized;
            Model.transform.forward = directionToHit;
            ShootDirection = directionToHit;
       }
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
    public Vector3 GetRaycastOrigin()
    {
        Vector3 origin = ShootSystem.transform.position;
        if(ShootConfig.ShootType == ShootType.FromCamera)
        {
            origin =  ActiveCamera.transform.position + ActiveCamera.transform.forward * Vector3.Distance(
                ActiveCamera.transform.position,
                ShootSystem.transform.position
            );
        }
        return origin;
    }
    public Vector3 GetGunForward()
    {
        return Model.transform.forward;
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
