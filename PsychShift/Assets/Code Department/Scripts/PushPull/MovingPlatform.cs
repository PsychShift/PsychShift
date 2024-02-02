using Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovingPlatform : MonoBehaviour
{
    private Dictionary<CharacterController, Vector3> characterLossyScale;
    private void Awake()
    {
        characterLossyScale = new Dictionary<CharacterController, Vector3>();
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders) 
            { 
                if (collider.isTrigger) return;
            }
        }

        BoxCollider triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = colliders[0].size * 1.2f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController controller))
        {
            //controller.transform.SetParent(transform, true);
            characterLossyScale.Add(controller, controller.transform.lossyScale);
            controller.transform.parent = transform;
            Debug.Log("Added " + characterLossyScale.Count);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterController controller))
        {
            //controller.transform.SetParent(null, false);
            controller.transform.parent = null;
            controller.transform.localScale = characterLossyScale[controller];
            characterLossyScale.Remove(controller);
            Debug.Log("removed " + characterLossyScale.Count);
        }
    }
}
