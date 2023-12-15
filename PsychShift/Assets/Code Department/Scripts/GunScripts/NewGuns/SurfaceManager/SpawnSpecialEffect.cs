using UnityEngine;
using System.Collections.Generic;

namespace ImpactSystem.Effects
{
    [CreateAssetMenu(menuName = "Impact System/Spawn Special Effect", fileName = "SpawnSpecialEffect")]
    public class SpawnSpecialEffect : ScriptableObject
    {
        public AbstractSpecialEffect SpecialEffect;
        public float Probability = 1;
        public bool RandomizeRotation;
        [Tooltip("Zero values will lock the rotation on that axis. Values up to 360 are sensible for each X,Y,Z")]
        public Vector3 RandomizedRotationMultiplier = Vector3.zero;
    }
}
