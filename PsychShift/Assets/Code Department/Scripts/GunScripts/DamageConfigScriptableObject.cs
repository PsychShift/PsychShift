using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
[CreateAssetMenu(fileName = "Damage Config", menuName = "OldGuns/Damage Config", order = 1)]
public class DamageConfigScriptableObject : ScriptableObject
{
    //public int Damage;
    //public Vector2Int MinMaxDamage;
    public MinMaxCurve DamageCurve;

    private void Reset()
    {
        DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }
    public int GetDamage(float Distane = 0)
    {
        return Mathf.CeilToInt(DamageCurve.Evaluate(Distane, Random.value));
    }
}
