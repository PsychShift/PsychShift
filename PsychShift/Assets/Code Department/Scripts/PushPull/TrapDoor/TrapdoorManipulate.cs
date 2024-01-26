using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrapdoorManipulate : MonoBehaviour, IManipulate
{
    public bool IsInteracted { get; set; }
    public bool CanInteract { get; set; }

    [SerializeField] bool canUseMoreThanOnce;
    private GameObject hinge1;
    private GameObject hinge2;

    private HingeJoint hingeJoint1;
    private HingeJoint hingeJoint2;

    private Quaternion originalRotation1;
    private Quaternion originalRotation2;

    private JointLimits limits;


    private void Start()
    {
        transform.tag = "Manipulatable";
        CanInteract = true;
        IsInteracted = false;


        hinge1 = gameObject.transform.GetChild(0).gameObject;
        hinge2 = gameObject.transform.GetChild(1).gameObject;

        hingeJoint1 = hinge1.GetComponent<HingeJoint>();
        hingeJoint2 = hinge2.GetComponent<HingeJoint>();

        limits.min = 0;
        limits.max = 0;
        hingeJoint1.limits = limits;
        hingeJoint2.limits = limits;
        
        hingeJoint1.useLimits = true;
        hingeJoint2.useLimits = true;

        originalRotation1 = hinge1.transform.rotation;
        originalRotation2 = hinge2.transform.rotation;
    }
    public void Interacted()
    {
        if(!IsInteracted)
            Interact();
        else
            ReverseInteract();
    }

    public void Interact()
    {
        if(CanInteract)
        {
            Open();
            IsInteracted = true;
        }
    }

    public void ReverseInteract()
    {
        if(CanInteract)
        {
            StartCoroutine(Close(2));
            IsInteracted = false;
        }
    }
    private void Open()
    {
        limits.max = -100;
        hingeJoint1.limits = limits;
        hingeJoint2.limits = limits;

        hingeJoint1.useLimits = true;
        hingeJoint2.useLimits = true;
    }
    private IEnumerator Close(float duration)
    {
        hinge1.GetComponent<Collider>().enabled = false;
        hinge2.GetComponent<Collider>().enabled = false;
        CanInteract = false;
        hingeJoint1.useLimits = false;
        hingeJoint2.useLimits = false;


        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;

            float t = elapsedTime / duration; // Interpolation factor.
            float journeyFraction = (Time.time - startTime) / duration;
            hinge1.transform.rotation = Quaternion.Lerp(hinge1.transform.rotation, originalRotation1, journeyFraction);
            hinge2.transform.rotation = Quaternion.Lerp(hinge2.transform.rotation, originalRotation2, journeyFraction);

            yield return null;
        }

        if (canUseMoreThanOnce)
        {
            CanInteract = true;
            limits.min = 0;
            limits.max = 0;
            hingeJoint1.limits = limits;
            hingeJoint2.limits = limits;

            hingeJoint1.useLimits = true;
            hingeJoint2.useLimits = true;
            hinge1.GetComponent<Collider>().enabled = true;
            hinge2.GetComponent<Collider>().enabled = true;
        }
    }
}
