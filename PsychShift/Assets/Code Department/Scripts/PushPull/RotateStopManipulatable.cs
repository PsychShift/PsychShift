using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStopManipulatable : MonoBehaviour, IManipulate
{
    [SerializeField] private float freezeTime = 1f;
    private float direction = 1f;

    [SerializeField] private bool rotateX;
    [SerializeField] private float rotateXSpeed = 1f;
    [SerializeField] private bool rotateY;
    [SerializeField] private float rotateYSpeed = 1f;
    [SerializeField] private bool rotateZ;
    [SerializeField] private float rotateZSpeed = 1f;

    [SerializeField]private bool canInteract = false;
    public bool IsInteracted { get; set; }
    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }


    void Start()
    {
        CanInteract = true;
        transform.tag = "Manipulatable";
        StartCoroutine(Rotate());
    }
    public void Interacted()
    {
        if(!CanInteract) return;

        Interact();
    }

    public void Interact()
    {
        StartCoroutine(Wait());
    }

    public void ReverseInteract()
    {
        
    }


    public IEnumerator Rotate()
    {
        CanInteract = true;
        while(CanInteract)
        {
            float x = rotateX ? rotateXSpeed * direction * Time.fixedDeltaTime : 0f; 
            float y = rotateY ? rotateYSpeed * direction * Time.fixedDeltaTime : 0f; 
            float z = rotateZ ? rotateZSpeed * direction * Time.fixedDeltaTime : 0f;
            
            transform.Rotate(x, y, z, Space.Self);
            yield return null;
        }
    }
    public IEnumerator Wait()
    {
        CanInteract = false; 
        yield return new WaitForSeconds(freezeTime);
        StartCoroutine(Rotate());
    }
}
