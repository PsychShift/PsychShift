using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyCardModifier : AbstractEnemyModifier
{
    Transform colliderParent;
    BoxCollider _collider;

    KeyCardScript keyCardScript;
    [HideInInspector]public EnemyBrain brain;
    public override void ApplyModifier(EnemyBrain brain)
    {
        this.brain = brain;
        keyCardScript = gameObject.AddComponent<KeyCardScript>();
        // add a new box collider
        colliderParent = new GameObject().transform;
        colliderParent.parent = transform;

        _collider = colliderParent.AddComponent<BoxCollider>();
        // set to trigger
        _collider.isTrigger = true;

        // set its tag
        colliderParent.tag = "KeyCard";
        colliderParent.gameObject.layer = LayerMask.NameToLayer("KeyCard");
    }
}
