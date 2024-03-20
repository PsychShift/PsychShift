using System;
using UnityEngine;

namespace Guns.ImpactEffects
{
    [Serializable]
    public class Explode : AbstractAreaOfEffect
    {
        public Explode(float Radius, AnimationCurve DamageFalloff, float BaseDamage, int MaxEnemiesAffected) :
            base(Radius, DamageFalloff, BaseDamage, MaxEnemiesAffected) { }
    }
}
