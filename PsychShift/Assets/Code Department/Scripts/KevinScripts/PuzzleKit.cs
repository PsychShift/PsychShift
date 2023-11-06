using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleKit : MonoBehaviour
{
    [TextArea]
    public string Notes = "Use this script for creating puzzles. Select the bools at the bottom. One for action another for reaction. Use this script again on a new object for new puzzles"; 
    //USE THIS SCRIPT FOR EVERY PUZZLE. YOU WILL NEED A NEW OBJECT WITH THIS SCRIPT FOR FOLLOW UP PUZZLES 


    [TextArea]
    public string Notepad = "Add the god puzzle kit object here, mainly used for interaction puzzles.";
    //god box var
    [SerializeField]
    PuzzleKit godBoxRef;//if this is on an item it knows it's the god box
    private bool godBox;


    [TextArea]
    public string Notes2 = "Actions below, pressurePlate needs a collider set to trigger to work";
    //Actions
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool pressurePlate;
    [SerializeField]
    private bool shootObj;

    [TextArea]
    public string Notes3 = "Reactions below";
    //Reactions
    [SerializeField]
    private bool move;
    [SerializeField]
    private bool spawn;
    [SerializeField]
    private bool count;

    [TextArea]
    public string moveVariables = "Variables that need to be filled in for move below";
    //Move variables 
    [SerializeField]
    Transform targetLocation;
    [SerializeField]
    float speedOfMove;
    private float startTime;
    private float journeyLength;
    private bool movingActivated;

    [TextArea]
    public string spawnVariables = "Variables that need to be filled in for Spawn below";
    //Spawn variables
    [SerializeField]
    GameObject[] spawnObjects;
    [TextArea]
    public string countVariables = "Variables that need to be filled in for count below";
    //count variables
    [SerializeField]
    int amountToActivate;
    private bool activated;
    private int activateCount;//counts how many items are activated




    //Actions
        //Interact
        //Pressure plate
        //shoot it
    //Re actions
        //Move object
        //Spawn object
        //count down to how many need activating//also deactivates this one
    private void Awake() 
    {
        if(godBoxRef!= null)
        {
            godBox = false;
        }
        else
        {
            godBox = true;//if you don't have a god box reference YOU ARE THE GOD BOX
        }
        if(godBox)
        {
            startTime = Time.time;
            journeyLength = Vector3.Distance(transform.position, targetLocation.position);  
        }
  
    }
    public void Update()
    {
        if(movingActivated && godBox)
        {
            Vector3.Lerp(transform.position, targetLocation.position, speedOfMove);
            float distCovered = (Time.time - startTime) * speedOfMove;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetLocation.position, fracJourney);
            if(fracJourney >= 1)
            {
                movingActivated = false;
            }
        }
    }

    public void Interact()
    {
        if(interact)
        {
            if(move)
            {
                Move();
            }
            else if(spawn)
            {
                SpawnObject();
            }
            else if(count)
            {
                ThisActivate();
            }
        }
    }
/*     public void PressurePlate() //all done in onTriggerEnter
    {
        //detects how long you've been standing on something

    } */
/*     public void ShootIt()
    {
        //Detect if bullet makes impact and sets off reaction.

    } */


    public void Move()
    {
        //Moves object forward and back
        godBoxRef.movingActivated = true;

    }

    public void SpawnObject()
    {
        //Spawns object after a puzzle is complete 
    }
    public void ThisActivate()
    {
        //activates something when called
        //check if activated already
        //add to counter
        //Call this whenever an item is interacted with. NEEDS REF TO PUZZLE KIT
        if(godBox == false)
        {
            
            if(activated == false)//activates this object and sends number to godBox.
            {
                activated = true;
                godBoxRef.activateCount++;
                godBoxRef.ThisActivate();

            }
        }
        else
        {

            if(activateCount == amountToActivate)
            {
                //Do whatever action is marked 
                if(move)
                {
                    godBoxRef.Move();
                }
                else if(spawn)
                {
                    godBoxRef.SpawnObject();
                }
            }

        }
    }
    private void OnTriggerEnter(Collider other)//used for pressure plate
    {

/*         if(move)
        {
            Move();
        }
        else if(spawn)
        {
            SpawnObject();
        }
        else if(count)
        {
            //send over to pressure plate 
            
            ThisActivate();
        } */

        if(shootObj)
        {
            if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                Debug.Log("Shot Ow");
                if(move)
                {
                    Move();
                }
                else if(spawn)
                {
                    SpawnObject();
                }
                else if(count)
                {
                    //send over to pressure plate 
            
                    ThisActivate();
                }
            }
            //send to reaction
        }
        else if(pressurePlate)
        {
            //send to reaction
            
            if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Stepped on Plate");
                if(move)
                {
                    Move();
                }
                else if(spawn)
                {
                    SpawnObject();
                }
                else if(count)
                {
                    //send over to pressure plate 
                    ThisActivate();
                }
            }
        } 
    }
    /* private void OnCollisionEnter(Collision other) 
    {

    } */
}
