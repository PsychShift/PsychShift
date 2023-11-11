using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "OldGuns/Guns Trail Config", order = 4 )]
public class TrailConfigScriptableObject : ScriptableObject
{
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;
    public float MissDistance = 100f;//despawns bullets after a miss
    public float SimulationSpeed = 100f;
}
