using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementStats", menuName = "Character/GeneralMovementStats", order = 1)]
public class CharacterMovementStatsSO : ScriptableObject
{
    public float moveSpeed = 5f;
    public AbilityType[] abilities;
    
}
[System.Flags]
public enum AbilityType
{
    WallRun,
    DoubleJump,
    Dash
}
