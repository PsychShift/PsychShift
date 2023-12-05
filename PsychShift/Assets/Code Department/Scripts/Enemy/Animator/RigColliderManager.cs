using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigColliderManager : MonoBehaviour
{
    public List<Collider> Colliders;
    void OnEnable()
    {
        GetComponentsInChildren(Colliders);
    }
}
