using System;
using UnityEngine;

namespace Guns.ImpactEffects
{
    [Serializable]
    public class Explode : AbstractAreaOfEffect
    {
        public Explode(float Radius, AnimationCurve DamageFalloff, float BaseDamage, int MaxEnemiesAffected) :
            base(Radius, DamageFalloff, BaseDamage, MaxEnemiesAffected) { }

        public override void HandleImpact(Collider ImpactedObject, Vector3 HitPosition, Vector3 HitNormal, GunScriptableObject Gun)
        {
            base.HandleImpact(ImpactedObject, HitPosition, HitNormal, Gun);
            foreach (var hits in HitObjects)
            {
                if(hits.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(HitNormal * 10f);
                }
            }
            if(Hits > 0)
                Gun.gunSelector.Hit(false);
        }
    }
}
