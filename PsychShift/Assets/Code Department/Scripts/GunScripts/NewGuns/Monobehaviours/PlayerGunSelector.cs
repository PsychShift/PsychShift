using Guns.Modifiers;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour, IGunSelector
    {
        public Camera Camera;
        private static PlayerGunSelector instance;
        public static PlayerGunSelector Instance
        {
            get
            {
                return instance;
            }
        }

        [SerializeField] private Transform GunParent;

        [SerializeField] private PlayerIK InverseKinematics;

        [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;
        [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

        /// <summary>
        /// If you are not using the demo AttachmentController, you may want it to initialize itself on start.
        /// If you are configuring this separately using <see cref="SetupGun"/> then set this to false.
        /// </summary>
        [SerializeField] private bool InitializeOnStart = false;
        public HitEffects hitRef;
        
        void Awake()
        {
            if(Instance == null)
            {
                instance = this;
            }
        }
        public void SetupGun(GunScriptableObject Gun)
        {
            ActiveBaseGun = Gun;
            ActiveGun = Gun.Clone() as GunScriptableObject;
            ActiveGun.Spawn(GunParent, this,this, Camera);
            ActiveGun.ShootConfig.ShootType = ShootType.FromCamera;
            PlayerHands.Instance.SetHandPositions(ActiveGun);
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

        public void Hit(bool crit)
        {
            StartCoroutine(hitRef.HitReaction(crit));//IF WE GET CRIT CHANGE DIS
        }
    }
}