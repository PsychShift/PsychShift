using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImpactSystem.Effects
{
    public abstract class AbstractSpecialEffect : ScriptableObject
    {
        public abstract void PlaySpecialEffect(SpecialEffectManager manager);
    }
}
