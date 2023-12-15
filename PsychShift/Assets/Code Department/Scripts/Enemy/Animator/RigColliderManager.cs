using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigColliderManager : MonoBehaviour
{
    public List<ChildCollider> childColliders;
    public void SetUp(IDamageable parentDamageable)
    {
        childColliders = new List<ChildCollider>();
        Collider[] children = GetComponentsInChildren<Collider>();
        foreach (Collider collider in children)
        {
            if(collider.gameObject == gameObject) continue;
            ChildCollider childCollider = collider.gameObject.AddComponent<ChildCollider>();
            childCollider.SetUp(parentDamageable);
            childColliders.Add(childCollider);
        }
    }

    public void SwapTag(string tag)
    {
        foreach (var collider in childColliders)
        {
            collider.gameObject.GetComponent<ChildCollider>().SwapTag(tag);
        }
    }

    public void SwapLayer(string layer)
    {
        foreach (var collider in childColliders)
        {
            collider.gameObject.GetComponent<ChildCollider>().SwapLayer(layer);
        }
    }
}
