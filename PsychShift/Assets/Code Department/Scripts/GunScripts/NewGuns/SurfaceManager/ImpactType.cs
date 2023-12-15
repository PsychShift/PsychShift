using UnityEngine;

namespace ImpactSystem
{
    [CreateAssetMenu(menuName = "Impact System/Impact Type", fileName = "ImpactType")]
    public class ImpactType : ScriptableObject
    {
        public bool CheckForRenderer = true;
    }
}