using System;
using UnityEngine;

namespace Guns.ImpactEffects
{
    public interface ICollisionHandler
    {
        /// <summary>
        /// Performs arbitrary actions when a bullet (or any projectile) impacts the specified object
        /// </summary>
        /// <param name="ImpactedObject"></param>
        /// <param name="HitPosition"></param>
        /// <param name="HitNormal"></param>
        /// <param name="Gun"></param>
        void HandleImpact(
            Collider ImpactedObject,
            Vector3 HitPosition,
            Vector3 HitNormal,
            GunScriptableObject Gun
        );
    }
}
