using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsScriptableObject", menuName = "RecoilConfigScriptableObject", order = 0)]
public class RecoilConfigScriptableObject : ScriptableObject
{
    public float rotationSpeed = 6;
    public float returnSpeed = 25;
    public Vector3 recoilRotation = new Vector3(2, 0.3f, 0f);
}
