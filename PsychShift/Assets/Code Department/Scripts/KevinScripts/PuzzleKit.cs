using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleKit : MonoBehaviour
{
    [TextArea]
    public string Notes = "Use this script for creating puzzles. Select the bools at the bottom. One for action another for reaction. Use this script again on a new object for new puzzles"; 
    //USE THIS SCRIPT FOR EVERY PUZZLE. YOU WILL NEED A NEW OBJECT WITH THIS SCRIPT FOR FOLLOW UP PUZZLES 

    //EFFECTS FOR SHOOT AND PLATE
    public ParticleSystem effectForAct;

    //EFFECT FOR REACTION
    public ParticleSystem effectForGod;
    [TextArea]
    public string Notepad = "Add the god puzzle kit object here, mainly used for interaction puzzles.";
    //god box var
    [SerializeField]
    PuzzleKit godBoxRef;//if this is on an item it knows it's the god box
    private bool godBox;


    [TextArea]
    public string Notes2 = "Actions below, pressurePlate/shoot needs a collider set to trigger to work, set a count number to tell code how many objects need to be activated before reaction.";
    //Actions
    [SerializeField]
    private bool stopSpawn;
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool pressurePlate;
    [SerializeField]
    private bool shootObj;
    [SerializeField]//Add this number on god box
    int amountToActivate;
    private bool activated;
    private int activateCount=0;

    [TextArea]
    public string Notes3 = "Reactions below";
    //Reactions
    [SerializeField]
    private bool move;
    [SerializeField]
    private bool spawn;
    private bool fall;

    [TextArea]
    public string moveVariables = "Variables that need to be filled in for move below";
    //Move variables 
    //[SerializeField]
    //Transform targetLocation;
    //[SerializeField] private Vector3 startPosition; // Initial position of the object
    [SerializeField] private Vector3 endPosition; 
    [SerializeField]
    float speedOfMove;
    private float startTime;
    private float journeyLength;
    private bool movingActivated;

    [TextArea]
    public string spawnVariables = "Variables that need to be filled in for Spawn below, set objects and location in godBox";
    //Spawn variables
    [SerializeField]
    GameObject[] spawnObjects;
    [SerializeField]
    Transform locationOfSpawn;
    [SerializeField]
    bool isSpawnInf;
    [SerializeField]
    int howManySpawn;
    int spawnCount;

    //shut off spawners after a bit 
    [SerializeField]
    GameObject[] spawnPoints;
    public bool puzzleComplete;
    public AudioSource Beep;
    public AudioClip soundClip;
    [TextArea]
    public string NotepadBoss = "Variables for Boss stuff";
    public bool isBossBox;
    public EncounterTracker bossContainerRef;
    /* public float puzzleCompletionTimer;//
    public bool puzzleTimer;
    private bool puzzleTimeBegan; */

    /* public delegate void OnPuzzleDone();
public static event OnPuzzleDone PuzzleDone; */




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
            if(effectForAct== null)
            {
                Debug.LogError("UH OH NO EFFECTS FOR THIS ACTION OBJ DETECTED LUL(HINT IM ONE OF THE PUZZLE KIT OBJ, I'M NOT THE GOD ONE THO) "+ gameObject.name);
            }
        }
        else
        {
            godBox = true;//if you don't have a god box reference YOU ARE THE GOD BOX
        }
        if(godBox)
        {
            startTime = Time.time;
            journeyLength = Vector3.Distance(transform.position, endPosition);
            if(effectForGod==null)
            {
                Debug.LogError("UH OH YOU FORGOT GOD BOX EFFECT LUL: "+ gameObject.name);
            } 
        }
    }
    public void Update()
    {
        if(movingActivated && godBox)
        {
            Vector3.Lerp(transform.position, endPosition, speedOfMove);
            float distCovered = (Time.time - startTime) * speedOfMove;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, endPosition, fracJourney);
            if(fracJourney >= 1)
            {
                movingActivated = false;
            }
        }
        /* if(puzzleTimeBegan)
        {

        } */
    }


    private void Move()
    {
        if(effectForGod!=null)
        {
            Instantiate(effectForGod,transform.position, Quaternion.identity);
        }
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        //Moves object forward and back
        Debug.Log("Moving");
        movingActivated = true;
        puzzleComplete = true;
        //PuzzleDone?.Invoke();
    }

    private void SpawnObject()
    {
            if(effectForGod!=null)
            {
                Instantiate(effectForGod,transform.position, Quaternion.identity);
            }
            if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
            for(int i = 0; i< spawnObjects.Length; i++) 
                Instantiate(spawnObjects[i], locationOfSpawn.position, Quaternion.identity);
            
            puzzleComplete = true;
       
     
    }
    private void StopSpawn()
    {
        for(int i =0; i<spawnPoints.Length;i++)
        {
            spawnPoints[i].SetActive(false);
        }
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
                if(soundClip!=null)
                    Beep.PlayOneShot(soundClip);
                if(effectForAct!=null)  
                    Instantiate(effectForAct,transform.position, Quaternion.identity); 
                Collider turnOff=gameObject.GetComponent<Collider>();
                godBoxRef.activateCount++;
                turnOff.enabled = false;
                Debug.Log("Activated: " + godBoxRef.activateCount);
                godBoxRef.ThisActivate();
            }
        }
        else if(godBox == true)
        {
            if(activateCount == amountToActivate)//runs when called to check if everything is activated
            {
                Debug.Log("Time2choose");
                //Do whatever action is marked 
                if(move)
                {
                    Debug.Log("Move");
                    Move();
                }
                else if(spawn)
                {
                    SpawnObject();
                }
                if(isBossBox)
                {
                    bossContainerRef.AddPuzzle();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)//used for pressure plate
    {
        Debug.Log("HERE ON TRIGGER");
        if(shootObj)
        {
            if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                ThisActivate();
            }
            //send to reaction
        }
        else if(pressurePlate)
        {
            //send to reaction
            
            if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                ThisActivate();
            }
        } 
    }

    public void ShootHitScan()
    {
        ThisActivate();
    }

    private void PlaySound(AudioClip beep)
    {
        Beep.PlayOneShot(beep);
    }
}
