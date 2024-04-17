using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using ImpactSystem;
using Guns.ImpactEffects;
using Guns.Stats;

namespace Guns
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject, System.ICloneable
    {
        public ImpactType ImpactType;
        public GunType Type;
        public string Name;
        public GameObject ModelPrefab;
        [HideInInspector] public List<HandsOrientation> HandOrientations;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        

        public DamageConfigScriptableObject DamageConfig;
        public ShootConfigScriptableObject ShootConfig;
        public AmmoConfigScriptableObject AmmoConfig;
        public TrailConfigScriptableObject TrailConfig;
        public AudioConfigScriptableObject AudioConfig;
        public BulletPenetrationConfigScriptableObject BulletPenConfig;
        public CharacterStatsScriptableObject CharacterConfig;

        public AnimatorOverrideController AnimatorOverride;



        public ICollisionHandler[] BulletImpactEffects = new ICollisionHandler[0];

        private MonoBehaviour ActiveMonoBehaviour;
        private AudioSource ShootingAudioSource;
        public GameObject Model;
        private Camera ActiveCamera;
        private float LastShootTime;
        private float InitialClickTime;
        private float StopShootingTime;

        public ParticleSystem ShootSystem;
        private ObjectPool<TrailRenderer> TrailPool;
        private ObjectPool<Bullet> BulletPool;
        private bool LastFrameWantedToShoot;
        private bool isEnemyMaybe;
        private Animator gunAnim;
        

        /* public delegate void OnSomethingHitDelegate(IDamageable damageable);
        public event OnSomethingHitDelegate OnSomethingHit; */
        private IGunSelector gunSelector;


        /// <summary>
        /// Spawns the Gun Model into the scene
        /// </summary>
        /// <param name="Parent">Parent for the gun model</param>
        /// <param name="ActiveMonoBehaviour">An Active MonoBehaviour that can have Coroutines attached to them.
        /// <param name="Camera">The camera to raycast from. Required if <see cref="ShootConfigScriptableObject.ShootType"/> = <see cref="ShootType.FromCamera"/></paramref>
        /// The input handling script is a good candidate for this.
        /// </param>
        public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour, IGunSelector gunSelector ,Camera Camera = null)
        {
            this.ActiveMonoBehaviour = ActiveMonoBehaviour;
            this.gunSelector = gunSelector;

            TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
            if (!ShootConfig.IsHitscan)
            {
                BulletPool = new ObjectPool<Bullet>(CreateBullet);
            }

            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(Parent, false);
            Model.transform.localPosition = SpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
            gunAnim = Model.GetComponent<AnimatorFind>().animator;

            ActiveCamera = Camera;

            ShootingAudioSource = Model.GetComponent<AudioSource>();
            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        }

        public List<HandsOrientation> GetHandOrientations()
        {
            if(Model.TryGetComponent(out SetHands setHands))
            {
                HandOrientations = setHands.hands;
                return HandOrientations;
            }
            else
                Debug.LogError("No SetHands component found on gun model");
            return null;
        }

        /// <summary>
        /// Despawns the active gameobjects and cleans up pools.
        /// </summary>
        public void Despawn()
        {
            // We do a bunch of other stuff on the same frame, so we really want it to be immediately destroyed, not at Unity's convenience.
            Model.SetActive(false);
            Destroy(Model);
            TrailPool.Clear();
            if (BulletPool != null)
            {
                BulletPool.Clear();
            }

            ShootingAudioSource = null;
            ShootSystem = null;
        }

        /// <summary>
        /// Used to override the Camera provided in <see cref="Spawn(Transform, MonoBehaviour, Camera)"/>.
        /// Cameras are only required if 
        /// </summary>
        /// <param name="Camera"></param>
        public void UpdateCamera(Camera Camera)
        {
            ActiveCamera = Camera;
        }

        /// <summary>
        /// Expected to be called every frame
        /// </summary>
        /// <param name="WantsToShoot">Whether or not the player is trying to shoot</param>
        public void Tick(bool WantsToShoot)
        {
            Model.transform.localRotation = Quaternion.Lerp(
                Model.transform.localRotation,
                Quaternion.Euler(SpawnRotation),
                Time.deltaTime * ShootConfig.RecoilRecoverySpeed
            );

            if (WantsToShoot)
            {
                LastFrameWantedToShoot = true;
                //gunAnim.SetInteger("Fire",1);
                TryToShoot();
            }

            if (!WantsToShoot && LastFrameWantedToShoot)
            {
                StopShootingTime = Time.time;
                gunAnim.SetInteger("Fire",0);
                LastFrameWantedToShoot = false;
            }
        }

        /// <summary>
        /// Plays the reloading audio clip if assigned.
        /// Expected to be called on the first frame that reloading begins
        /// </summary>
        public void StartReloading()
        {
            AudioConfig.PlayReloadClip(ShootingAudioSource);
        }

        /// <summary>
        /// Handle ammo after a reload animation.
        /// ScriptableObjects can't catch Animation Events, which is how we're determining when the
        /// reload has completed, instead of using a timer
        /// </summary>
        public void EndReload()
        {
            AmmoConfig.Reload();
        }

        /// <summary>
        /// Whether or not this gun can be reloaded
        /// </summary>
        /// <returns>Whether or not this gun can be reloaded</returns>
        public bool CanReload()
        {
            return AmmoConfig.CanReload();
        }

        /// <summary>
        /// Performs the shooting raycast if possible based on gun rate of fire. Also applies bullet spread and plays sound effects based on the AudioConfig.
        /// </summary>
        public void TryToShoot(bool isEnemy = false)
        {
            isEnemyMaybe=isEnemy;//sets if enemy for whole script
            if (Time.time - LastShootTime - ShootConfig.FireRate > Time.deltaTime)
            {
                float lastDuration = Mathf.Clamp(
                    0,
                    (StopShootingTime - InitialClickTime),
                    ShootConfig.MaxSpreadTime
                );
                float lerpTime = (ShootConfig.RecoilRecoverySpeed - (Time.time - StopShootingTime))
                                 / ShootConfig.RecoilRecoverySpeed;

                InitialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
            }

            if (Time.time > ShootConfig.FireRate + LastShootTime)
            {
                // for ShootConfig.BulletsPerShot
                // Do this
                LastShootTime = Time.time;
                if (AmmoConfig.CurrentClipAmmo == 0)
                {
                    AudioConfig.PlayOutOfAmmoClip(ShootingAudioSource);
                    gunAnim.SetInteger("Fire",0);
                    return;
                }
                gunAnim.SetInteger("Fire",1);
                //gunAnim.SetInteger("Fire",0);
                ShootSystem.Play();
                AudioConfig.PlayShootingClip(ShootingAudioSource, AmmoConfig.CurrentClipAmmo == 1);

                if(!isEnemy)
                        AmmoConfig.CurrentClipAmmo--;

                for(int i = 0; i<ShootConfig.BulletsPerShot; i++)
                {
                    Vector2 spreadAmount = ShootConfig.GetSpread(Time.time - InitialClickTime);
                    Vector3 shootDirection;

                    //Model.transform.forward += Model.transform.TransformDirection(spreadAmount); FOR NOW
                    if (ShootConfig.ShootType == ShootType.FromGun)
                    {
                        shootDirection.x = spreadAmount.x + ShootSystem.transform.forward.x;
                        shootDirection.y = spreadAmount.y + ShootSystem.transform.forward.y;
                        shootDirection.z = ShootSystem.transform.forward.z;
                    }
                    else
                    {
                        shootDirection = ActiveCamera.transform.forward +
                                        ActiveCamera.transform.TransformDirection(new Vector3(spreadAmount.x, spreadAmount.y, ActiveCamera.transform.forward.z));
                    }

                    

                    if (ShootConfig.IsHitscan)
                    {
                        DoHitscanShoot(shootDirection, GetRaycastOrigin(), ShootSystem.transform.position);
                    }
                    else
                    {
                        DoProjectileShoot(shootDirection);
                    }
                }
                
            }
        }

        /// <summary>
        /// Generates a live Bullet instance that is launched in the <paramref name="ShootDirection"/> direction
        /// with velocity from <see cref="ShootConfigScriptableObject.BulletSpawnForce"/>.
        /// </summary>
        /// <param name="ShootDirection"></param>
        private void DoProjectileShoot(Vector3 ShootDirection, bool isEnemy = false)
        {
            Bullet bullet = BulletPool.Get();
            bullet.gameObject.SetActive(true);
            bullet.gameObject.layer = isEnemy ? LayerMask.NameToLayer("EnemyBullet") : LayerMask.NameToLayer("Bullet");
            bullet.OnCollision += HandleBulletCollision;
            /* if(isEnemyMaybe == true)
                bullet.gameObject.layer = 17;
            else if(isEnemyMaybe == false)
                bullet.gameObject.layer = 14; */

            // We have to ensure if shooting from the camera, but shooting real proejctiles, that we aim the gun at the hit point
            // of the raycast from the camera. Otherwise the aim is off.
            // When shooting from the gun, there's no need to do any of this because the recoil is already handled in TryToShoot
            if (ShootConfig.ShootType == ShootType.FromCamera
                && Physics.Raycast(
                    GetRaycastOrigin(),
                    ShootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                Vector3 directionToHit = (hit.point - ShootSystem.transform.position).normalized;
                Model.transform.forward = directionToHit;
                ShootDirection = directionToHit;
            }

            bullet.transform.position = ShootSystem.transform.position;
            bullet.Spawn(ShootDirection * ShootConfig.BulletSpawnForce);

            TrailRenderer trail = TrailPool.Get();
            if (trail != null)
            {
                trail.transform.SetParent(bullet.transform, false);
                trail.transform.localPosition = Vector3.zero;
                trail.emitting = true;
                trail.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Performs a Raycast to determine if a shot hits something. Spawns a TrailRenderer
        /// and will apply impact effects and damage after the TrailRenderer simulates moving to the
        /// hit point. 
        /// See <see cref="PlayTrail(Vector3, Vector3, RaycastHit)"/> for impact logic.
        /// </summary>
        /// <param name="ShootDirection"></param>
        private void DoHitscanShoot(Vector3 ShootDirection, Vector3 Origin, Vector3 TrailOrigin, int Iteration = 0)
        {
            if (Physics.Raycast(
                    Origin,
                    ShootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                HandleBulletImpact(
                    Vector3.Distance(hit.point, TrailOrigin),
                    hit.point,
                    hit.normal,
                    hit.collider,
                    Iteration
                );
                
                /* if(hit.collider.tag=="Swapable")
                {
                    AudioConfig.PlayHitClip(ShootingAudioSource);
                    //Start UI animation if it came from player.
                    //Need UI reference 
                    // 
                    
                } */
                    
                /* else if(hit.collider.tag == "Player")
                    AudioConfig.PlayHitClipEnemy(ShootingAudioSource); */
                /* if(hit.collider.tag == "PuzzleShoot") // I made the Puzzle Kit an IDamageable, so we don't have to check for the tag anymore, it will just play if it got hit straight from the puzzle script
                {
                    PuzzleKit pRef = hit.collider.gameObject.GetComponent<PuzzleKit>();
                    pRef.ShootHitScan();
                } */
                /* ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        TrailOrigin,
                        hit.point,
                        hit,
                        Iteration
                    )
                );
                
            }
            else
            {
                HandleBulletImpact(
                    TrailConfig.MissDistance,
                    TrailOrigin + (ShootDirection * TrailConfig.MissDistance),
                    ShootDirection,
                    null,
                    Iteration
                );
                /* ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        TrailOrigin,
                        TrailOrigin + (ShootDirection * TrailConfig.MissDistance),
                        new RaycastHit(),
                        Iteration
                    )
                ); */
            }
        }

        /// <summary>
        /// Returns the proper Origin point for raycasting based on <see cref="ShootConfigScriptableObject.ShootType"/>
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRaycastOrigin()
        {
            Vector3 origin = ShootSystem.transform.position;

            if (ShootConfig.ShootType == ShootType.FromCamera)
            {
                origin = ActiveCamera.transform.position
                         + ActiveCamera.transform.forward * Vector3.Distance(
                             ActiveCamera.transform.position,
                             ShootSystem.transform.position
                         );
            }

            return origin;
        }

        /// <summary>
        /// Returns the forward of the spawned gun model
        /// </summary>
        /// <returns></returns>
        public Vector3 GetGunForward()
        {
            return Model.transform.forward;
        }

        /// <summary>
        /// Plays a bullet trail/tracer from start/end point. 
        /// If <paramref name="Hit"/> is not an empty hit, it will also play an impact using the <see cref="SurfaceManager"/>.
        /// </summary>
        /// <param name="StartPoint">Starting point for the trail</param>
        /// <param name="EndPoint">Ending point for the trail</param>
        /// <param name="Hit">The hit object. If nothing is hit, simply pass new RaycastHit()</param>
        /// <returns>Coroutine</returns>
        private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit, int Iteration = 0)
        {
            TrailRenderer instance = TrailPool.Get();
            instance.gameObject.SetActive(true);
            instance.transform.position = StartPoint;
            yield return null; // avoid position carry-over from last frame if reused

            instance.emitting = true;

            float distance = Vector3.Distance(StartPoint, EndPoint);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    StartPoint,
                    EndPoint,
                    Mathf.Clamp01(1 - (remainingDistance / distance))
                );
                remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

                yield return null;
            }

            instance.transform.position = EndPoint;

            if (Hit.collider != null)
            {
                HandleBulletImpact(distance, EndPoint, Hit.normal, Hit.collider, Iteration);
            }

            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            TrailPool.Release(instance);

            if (BulletPenConfig != null && BulletPenConfig.MaxObjectsToPenetrate > Iteration)
            {
                yield return null;
                Vector3 direction = (EndPoint - StartPoint).normalized;
                Vector3 backCastOrigin = Hit.point + direction * BulletPenConfig.MaxPenetrationDepth;

                if (Physics.Raycast(
                        backCastOrigin,
                        -direction,
                        out RaycastHit hit,
                        BulletPenConfig.MaxPenetrationDepth,
                        ShootConfig.HitMask
                    ))
                {
                    Vector3 penetrationOrigin = hit.point;
                    direction += new Vector3(
                        Random.Range(-BulletPenConfig.AccuracyLoss.x, BulletPenConfig.AccuracyLoss.x),
                        Random.Range(-BulletPenConfig.AccuracyLoss.y, BulletPenConfig.AccuracyLoss.y),
                        Random.Range(-BulletPenConfig.AccuracyLoss.z, BulletPenConfig.AccuracyLoss.z)
                    );

                    DoHitscanShoot(direction, penetrationOrigin, penetrationOrigin, Iteration + 1);
                }
            }
        }

        /// <summary>
        /// Callback handler for <see cref="Bullet.OnCollision"/>. Disables TrailRenderer, releases the 
        /// Bullet from the BulletPool, and applies impact effects if <paramref name="Collision"/> is not null.
        /// </summary>
        /// <param name="Bullet"></param>
        /// <param name="Collision"></param>
        private void HandleBulletCollision(Bullet Bullet, Collision Collision, int ObjectsPenetrated)
        {
            Bullet.OnCollision -= HandleBulletCollision;
            TrailRenderer trail = Bullet.GetComponentInChildren<TrailRenderer>();

            if (Collision != null && BulletPenConfig != null &&
                BulletPenConfig.MaxObjectsToPenetrate > ObjectsPenetrated)
            {
                Vector3 direction = (Bullet.transform.position - Bullet.SpawnLocation).normalized;
                ContactPoint contact = Collision.GetContact(0);
                Vector3 backCastOrigin = contact.point + direction * BulletPenConfig.MaxPenetrationDepth;

                if (Physics.Raycast(
                        backCastOrigin,
                        -direction,
                        out RaycastHit hit,
                        BulletPenConfig.MaxPenetrationDepth,
                        ShootConfig.HitMask
                    ))
                {
                    direction += new Vector3(
                        Random.Range(-BulletPenConfig.AccuracyLoss.x, BulletPenConfig.AccuracyLoss.x),
                        Random.Range(-BulletPenConfig.AccuracyLoss.y, BulletPenConfig.AccuracyLoss.y),
                        Random.Range(-BulletPenConfig.AccuracyLoss.z, BulletPenConfig.AccuracyLoss.z)
                    );
                    Bullet.transform.position = hit.point + direction * 0.01f;

                    Bullet.Rigidbody.velocity = Bullet.SpawnVelocity - direction;
                }
                else
                {
                    DisableTrailAndBullet(trail, Bullet);
                }
            }
            else
            {
                DisableTrailAndBullet(trail, Bullet);
            }

            if (Collision != null)
            {
                ContactPoint contactPoint = Collision.GetContact(0);

                HandleBulletImpact(
                    Vector3.Distance(contactPoint.point, Bullet.SpawnLocation),
                    contactPoint.point,
                    contactPoint.normal,
                    contactPoint.otherCollider,
                    ObjectsPenetrated
                );
            }
        }

        private void DisableTrailAndBullet(TrailRenderer Trail, Bullet Bullet)
        {
            if (Trail != null)
            {
                Trail.transform.SetParent(null, true);
                ActiveMonoBehaviour.StartCoroutine(DelayedDisableTrail(Trail));
            }

            Bullet.gameObject.SetActive(false);
            BulletPool.Release(Bullet);
        }

        /// <summary>
        /// Disables the trail renderer based on the <see cref="TrailConfigScriptableObject.Duration"/> provided
        ///and releases it from the<see cref="TrailPool"/>
        /// </summary>
        /// <param name="Trail"></param>
        /// <returns></returns>
        private IEnumerator DelayedDisableTrail(TrailRenderer Trail)
        {
            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            Trail.emitting = false;
            Trail.gameObject.SetActive(false);
            TrailPool.Release(Trail);
        }

        /// <summary>
        /// Calls <see cref="SurfaceManager.HandleImpact(GameObject, Vector3, Vector3, ImpactType, int)"/> and applies damage
        /// if a damagable object was hit
        /// </summary>
        /// <param name="DistanceTraveled"></param>
        /// <param name="HitLocation"></param>
        /// <param name="HitNormal"></param>
        /// <param name="HitCollider"></param>
        private void HandleBulletImpact(
            float DistanceTraveled,
            Vector3 HitLocation,
            Vector3 HitNormal,
            Collider HitCollider,
            int ObjectsPenetrated = 0)
        {
            if(HitCollider == null) return;
            
            SurfaceManager.Instance.HandleImpact(
                HitCollider.transform.root.gameObject,
                HitLocation,
                HitNormal,
                ImpactType,
                0
            );

            if (HitCollider.TryGetComponent(out IDamageable damageable))
            {
                float maxPercentDamage = 1;
                if (BulletPenConfig != null && ObjectsPenetrated > 0)
                {
                    for (int i = 0; i < ObjectsPenetrated; i++)
                    {
                        maxPercentDamage *= BulletPenConfig.DamageRetentionPercentage;
                    }
                }
                //Debug.Log("damgeplez");
                float critMod = damageable.IsWeakPoint ? DamageConfig.CritModifier : 1;
                damageable.TakeDamage(DamageConfig.GetDamage(DistanceTraveled, maxPercentDamage) * critMod, Type);
                /* OnSomethingHit?.Invoke(damageable);
                if(OnSomethingHit== null)
                    Debug.Log("NULL AF"); */
                gunSelector.Hit(damageable.IsWeakPoint);
            }

            foreach (ICollisionHandler collisionHandler in BulletImpactEffects)
            {
                collisionHandler.HandleImpact(HitCollider, HitLocation, HitNormal, this);
            }
        }

        /// <summary>
        /// Creates a trail Renderer for use in the object pool.
        /// </summary>
        /// <returns>A live TrailRenderer GameObject</returns>
        private TrailRenderer CreateTrail()
        {
            GameObject instance = new GameObject("Bullet Trail");
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = TrailConfig.Color;
            trail.material = TrailConfig.Material;
            trail.widthCurve = TrailConfig.WidthCurve;
            trail.time = TrailConfig.Duration;
            trail.minVertexDistance = TrailConfig.MinVertexDistance;

            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }

        /// <summary>
        /// Creates a Bullet for use in the object pool.
        /// </summary>
        /// <returns>A live Bullet GameObject</returns>
        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(ShootConfig.BulletPrefab);
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.mass = ShootConfig.BulletWeight;

            return bullet;
        }

        public object Clone()
        {
            GunScriptableObject config = CreateInstance<GunScriptableObject>();

            config.ImpactType = ImpactType;
            config.Type = Type;
            config.Name = Name;
            config.name = name;

            config.DamageConfig = DamageConfig.Clone() as DamageConfigScriptableObject;
            config.ShootConfig = ShootConfig.Clone() as ShootConfigScriptableObject;
            config.AmmoConfig = AmmoConfig.Clone() as AmmoConfigScriptableObject;
            config.TrailConfig = TrailConfig.Clone() as TrailConfigScriptableObject;
            config.AudioConfig = AudioConfig.Clone() as AudioConfigScriptableObject;
            config.BulletPenConfig = BulletPenConfig.Clone() as BulletPenetrationConfigScriptableObject;
            config.CharacterConfig = CharacterConfig.Clone() as CharacterStatsScriptableObject;

            config.ModelPrefab = ModelPrefab;
            config.AnimatorOverride = AnimatorOverride;
            config.SpawnPoint = SpawnPoint;
            config.SpawnRotation = SpawnRotation;

            config.ShootConfig.PreCalculateBulletSpread(config.AmmoConfig.ClipSize);

            if(config.DamageConfig.IsExplosive)
            {
                config.BulletImpactEffects = new ICollisionHandler[]
                {
                    new Explode(
                        config.DamageConfig.Radius,
                        config.DamageConfig.DamageFalloff,
                        config.DamageConfig.BaseAOEDamage,
                        10)
                };
            }

            return config;
        }
    }

}

