using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerStateMachine : MonoBehaviour
{
    private StateMachine stateMachine;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        stateMachine = new StateMachine();

        // Create Instances of the states for the script to use
        // This uses a constructor made in the states script. The argument can be anything set in the script.
        var standardState = new StandardState(this);
    }
}
