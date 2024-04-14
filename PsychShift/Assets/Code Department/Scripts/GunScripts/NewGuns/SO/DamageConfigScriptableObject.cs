using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Guns
{
    [CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Config", order = 1)]
    public class DamageConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        public MinMaxCurve DamageCurve;
        public float CritModifier = 1.5f;

        [Header("Explosive")]
        public bool IsExplosive = false;
        public float Radius = 0;
        public AnimationCurve DamageFalloff;
        public float BaseAOEDamage = 0;

        private void Reset()
        {
            DamageCurve.mode = ParticleSystemCurveMode.Curve;
        }

        public float GetDamage(float Distance = 0, float DamageMultiplier = 1)
        {
            return Mathf.CeilToInt(
                DamageCurve.Evaluate(Distance, Random.value) * DamageMultiplier
            );
        }
        /* public float GetDamage(float Distance = 0, float DamageMultiplier = 1)
        {
            return Distance <= MinDist ? Damage * DamageMultiplier : Mathf.Lerp(Damage, MinDamage, (Distance - MinDist) / (MaxDist - MinDist)) * DamageMultiplier;
        } */

        public object Clone()
        {
            DamageConfigScriptableObject config = CreateInstance<DamageConfigScriptableObject>();

            config.DamageCurve = DamageCurve;
            config.CritModifier = CritModifier;
            config.IsExplosive = IsExplosive;
            config.Radius = Radius;
            config.DamageFalloff = DamageFalloff;
            config.BaseAOEDamage = BaseAOEDamage;

            return config;
        }
    }
}