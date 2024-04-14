using UnityEngine;
using Guns.Modifiers;
using System;
using UnityEngine.Animations.Rigging;

namespace Guns.Demo
{
    [DisallowMultipleComponent]
    public class EnemyGunSelector : MonoBehaviour, IGunSelector
    {
        public static float DamageReduction = 0.2f;
        public Camera Camera;

        public GunScriptableObject StartGun;
        public Transform GunParent;

        //[SerializeField] private PlayerIK InverseKinematics;

        [Space] 
        [Header("Runtime Filled")] 
        [SerializeField]private GunScriptableObject _activeGun;
        public GunScriptableObject ActiveGun
        { 
            get { return _activeGun; } 
            
            private set
            {
                _activeGun = value;
            }
        }
        [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

        /// <summary>
        /// If you are not using the demo AttachmentController, you may want it to initialize itself on start.
        /// If you are configuring this separately using <see cref="SetupGun"/> then set this to false.
        /// </summary>
        [SerializeField] private bool InitializeOnStart = false;

        public event Action OnActiveGunSet;
        void OnEnable()
        {
            SetupGun(StartGun);
        }
        public void SetupGun(GunScriptableObject Gun)
        {
            ActiveBaseGun = Gun;
            ActiveGun = Gun.Clone() as GunScriptableObject;
            ActiveGun.Spawn(GunParent, this, this, Camera);

            ActiveGun.ShootConfig.SpreadType = BulletSpreadType.Simple;
            ActiveGun.ShootConfig.SpreadMultiplier = 10f;
            ActiveGun.ShootConfig.ShootType = ShootType.FromGun;

            ActiveGun.DamageConfig.DamageCurve.curveMultiplier = DamageReduction;
            
            /* ActiveGun.DamageConfig.Damage *= DamageReduction;
            ActiveGun.DamageConfig.MinDamage *= DamageReduction; */
            /* ActiveGun.DamageConfig.DamageCurve.constant *= DamageReduction;

            float dmg = ActiveGun.DamageConfig.DamageCurve.constant;
            float minDist = ActiveGun.DamageConfig.DamageCurve.curve.keys[1].time;
            float maxDist = ActiveGun.DamageConfig.DamageCurve.curve.keys[2].time;
            float minDmg = ActiveGun.DamageConfig.DamageCurve.curve.keys[2].value * DamageReduction;
            float slope = (minDmg - dmg) / (maxDist - minDist); // Slope between Vector2 2 and Vector2 3

            Keyframe[] frames = new Keyframe[3]
            {
                new(0, dmg, 0, 0),
                new(minDist, dmg, 0, slope),
                new(maxDist, minDmg, slope, 0)
            };
            ActiveGun.DamageConfig.DamageCurve.curve.keys = frames; */
  
            ActiveGun.Model.AddComponent<RigTransform>();
            OnActiveGunSet?.Invoke();
            /* InverseKinematics.SetGunStyle(ActiveGun.Type == GunType.Glock);
            InverseKinematics.Setup(GunParent); */
        }

        public void DespawnActiveGun()
        {
            if (ActiveGun != null)
            {
                ActiveGun.Despawn();
            }

            Destroy(ActiveGun);
        }

        public void PickupGun(GunScriptableObject Gun)
        {
            DespawnActiveGun();
            SetupGun(Gun);
        }

        public void ApplyModifiers(IModifier[] Modifiers)
        {
            DespawnActiveGun();
            SetupGun(ActiveBaseGun);

            foreach (IModifier modifier in Modifiers)
            {
                modifier.Apply(ActiveGun);
            }
        }

        public bool ShouldReload()
        {
            return ActiveGun.AmmoConfig.CurrentClipAmmo <= 0;
        }

        public void EnemyShoot()
        {
            ActiveGun.TryToShoot(true);
        }

        public void Hit()
        {
            
        }
    }
}