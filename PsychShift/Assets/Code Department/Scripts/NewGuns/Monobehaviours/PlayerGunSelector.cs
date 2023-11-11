using Guns.Modifiers;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour
    {
        public Camera Camera;
        [field: SerializeField] public GunType Gun { get; private set; }

        [SerializeField] private Transform GunParent;
        [field: SerializeField] public List<GunScriptableObject> Guns { get; private set; }
        [SerializeField] private PlayerIK InverseKinematics;

        [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;
        [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

        [SerializeField] private PlayerStateMachine PlayerStateMachine;

        /// <summary>
        /// If you are not using the demo AttachmentController, you may want it to initialize itself on start.
        /// If you are configuring this separately using <see cref="SetupGun"/> then set this to false.
        /// </summary>
        [SerializeField] private bool InitializeOnStart = false;

        private void SetupGun(GunScriptableObject Gun)
        {
            ActiveBaseGun = Gun;
            ActiveGun = Gun.Clone() as GunScriptableObject;
            ActiveGun.Spawn(GunParent, this, Camera);

            InverseKinematics.SetGunStyle(ActiveGun.Type == GunType.Glock);
            InverseKinematics.Setup(GunParent);
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
            this.Gun = Gun.Type;
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
    }
}