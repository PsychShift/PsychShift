using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : PuzzleKit
{
    [SerializeField]
    private GameObject beforeSwitch;
    [SerializeField]
    private GameObject afterSwitch;
    [SerializeField]
    private GameObject cutscene;
    [SerializeField]
    private GameObject greenButton;
    public override void ThisActivate()
    {
        doNotReact = true; 
        base.ThisActivate();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
