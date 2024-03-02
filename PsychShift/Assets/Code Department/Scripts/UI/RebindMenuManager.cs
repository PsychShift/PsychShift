using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    public InputActionReference MoveRef, JumpRef, FireRef, SlowRef, MindSwapRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        MoveRef.action.Disable();
        JumpRef.action.Disable();
        FireRef.action.Disable();
        SlowRef.action.Disable();
        MindSwapRef.action.Disable();
    }
    private void OnDisable()
    {
        MoveRef.action.Enable();
        JumpRef.action.Enable();
        FireRef.action.Enable();
        SlowRef.action.Enable();
        MindSwapRef.action.Enable();
    }
}
