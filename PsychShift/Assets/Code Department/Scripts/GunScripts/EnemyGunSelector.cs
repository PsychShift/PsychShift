using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Guns.Modifiers;

namespace Guns.Demo
{
    [DisallowMultipleComponent]
    public class EnemyGunSelector : MonoBehaviour
    {
        public Camera Camera;

        public GunScriptableObject StartGun;
        [SerializeField] private Transform GunParent;

        [SerializeField] private PlayerIK InverseKinematics;

        [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;
        [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

        /// <summary>
        /// If you are not using the demo AttachmentController, you may want it to initialize itself on start.
        /// If you are configuring this separately using <see cref="SetupGun"/> then set this to false.
        /// </summary>
        [SerializeField] private bool InitializeOnStart = false;
        void Start()
        {
            SetupGun(StartGun);
        }
        public void SetupGun(GunScriptableObject Gun)
        {
            ActiveBaseGun = Gun;
            ActiveGun = Gun.Clone() as GunScriptableObject;
            ActiveGun.Spawn(GunParent, this, Camera);

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
    }
}