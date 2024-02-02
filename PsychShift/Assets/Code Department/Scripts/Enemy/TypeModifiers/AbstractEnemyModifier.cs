using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyModifier : MonoBehaviour
{
    public abstract void ApplyModifier(EnemyBrain brain);
}
