using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingManipulatable : MonoBehaviour, IManipulate
{
    [SerializeField] private float rotationSpeed;
    private float direction = 1f;

    [SerializeField] private bool rotateX;
    [SerializeField] private bool rotateY;
    [SerializeField] private bool rotateZ;

    [SerializeField]private bool canInteract = false;
    public bool IsInteracted { get; set; }
    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }
    private bool forward = true;

    private void Awake() {
        
    }
    private void Start() {
        //StartCoroutine(WhileTest());
    }
    private void FixedUpdate() {
        
        direction = forward ? 1f : -1f;
        direction *= rotationSpeed;
        transform.Rotate(0f, 1 * direction, 0f, Space.Self);
    }

    public void Interacted()
    {
        Debug.Log("interacted");
        if(CanInteract)
            forward = !forward;
    }

    public void Interact()
    {
        
    }

    public void ReverseInteract()
    {
        
    }

    public IEnumerator WhileTest()
    {
        while(true)
        {
            Debug.Log("while");
            yield return new WaitForSeconds(1f);
        }
    }
}
/*     private void OnEnable() {
        
    }
    private void OnDisable() {
        
    }
    private void OnDestroy() {
        
    } */