using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleKit : MonoBehaviour, IDamageable
{
    
    [Header("All objects need these")]
    public AudioSource Beep;
    public AudioClip soundClip;

    [Header("Add and select these for action obj")]
    [SerializeField]
    PuzzleKit godBoxRef;
    public ParticleSystem effectForAct;

    
    //Actions
    [Header("How do you wanna interact?")]
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool pressurePlate;
    [SerializeField]
    private bool shootObj;

    [Header("Add and select these for GOD obj")]
    public ParticleSystem effectForGod;
    [SerializeField]//Add this number on god box
    int amountToActivate;
    public int activateCount=0;

    [Header("What do you want god to do?(can do multiple)")]
    [Header("Move options")]
    [SerializeField]
    private bool move;
    
    [SerializeField] private Vector3 endPosition;

    [SerializeField]
    float speedOfMove; 


    [Header("Spawn options")]
    [SerializeField]
    private bool spawn;
    
    [SerializeField]
    GameObject[] spawnObjects;
    [SerializeField]
    Transform locationOfSpawn;
    [SerializeField]
    int howManySpawn;


    [Header("Activate Options")]
    [SerializeField]
    private bool activate;
    
    [SerializeField]
    GameObject[] activateObject;

    [Header("Deactivate Options")]
    [SerializeField]
    private bool deactivate;
    
    [SerializeField]
    GameObject[] deactivateObject;

    [Header("Dissolve Options")]
    [SerializeField]
    private bool dissolveOBJ;
    
    public float dissolveDuration = 2;
    public float dissolveStrength;
    

    [Header("THIS IS FOR CHANGING BUTTON COLOR AND STUFF WHEN ACTIVATED")]
    [SerializeField]
    private bool changeObject;
    [Tooltip("GO is what it turns into")]
    [SerializeField]
    GameObject activatedObject;//WHAT WAS I THINKING AAAAAAAAAAAAAAAAAAAAAAAAAA



    //these are in script only
    private bool godBox;
    private bool activated;
    public bool puzzleComplete;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}

    public int MaxHealth { get { return 1; } }

    public bool IsWeakPoint { get; } = false;

    private bool destructObject;

    
    //Move Vars
    private float startTime;
    private float journeyLength;
    private bool movingActivated;


    //VARIABLE THAT NEED ORGANIZING
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
        CurrentHealth = MaxHealth;
        if(changeObject == false)
            destructObject = true;
        

    }
    public void Update()
    {
        /* if(moveOnStart)
        {
            Vector3.Lerp(transform.position, endPosition, speedOfStart);
            float distCovered = (Time.time - startTime) * speedOfStart;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, endPointStart, fracJourney);
            if(fracJourney >= 1)
            {
                moveOnStart = false;
            }
        } */

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
        

        movingActivated = true;
        puzzleComplete = true;
        #region 
        
        /* if(isAnim)
        {
            //PLAY animation here
            Debug.Log("Trying ANim");
            Animator godBoxAnim = gameObject.GetComponent<Animator>();
            godBoxAnim.SetBool("Move", true);
        }
        else
        { */ //GARBO CODE that I'm hoarding
        
        //Moves object forward and back
        //PuzzleDone?.Invoke();
        #endregion
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
    private void ActivateObject()
    {
        if(effectForGod!=null)
            {
                Instantiate(effectForGod,transform.position, Quaternion.identity);
            }
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        for(int i = 0; i< activateObject.Length;i++)
        {
            activateObject[i].SetActive(true);
        }
        
            
        
    }

    private void DeactivateObject()
    {
        if(effectForGod!=null)
            {
                Instantiate(effectForGod,transform.position, Quaternion.identity);
            }
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        for(int i = 0; i< deactivateObject.Length;i++)
        {
            deactivateObject[i].SetActive(false);
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
                //Destroy(this.gameObject);
                if(changeObject)
                {
                    activatedObject.SetActive(true);
                    this.gameObject.SetActive(false);  
                }
                else if(destructObject)
                    Destroy(this.gameObject);
                else if(dissolveOBJ)
                    StartDissolver();
                //Change color or object
            }
        }
        else if(godBox == true)
        {
            if(activateCount == amountToActivate)//runs when called to check if everything is activated
            {
                //Do whatever action is marked
                if(activate)
                {
                    ActivateObject();
                }
                if(deactivate)
                {
                    DeactivateObject();
                }
                else if(dissolveOBJ)
                    StartDissolver(); 

                
                if(move)
                {
                    Debug.Log("Move");
                    Move();
                }
                if(spawn)
                {
                    SpawnObject();
                }
                /* if(isBossBox)
                {
                    bossContainerRef.AddPuzzle();
                } */
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)//used for pressure plate
    {
        /* if(shootObj)
        {
            if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                ThisActivate();
            }
            //send to reaction
        }
        else  */
        if(pressurePlate)
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
        if(shootObj)
            ThisActivate();
    }
    public void InteractFeature()
    {
        if(interact)
            ThisActivate();
    }

    private void PlaySound(AudioClip beep)
    {
        Beep.PlayOneShot(beep);
    }

    private bool wasHit => CurrentHealth == 0;
    public void TakeDamage(int Damage)
    {
        if(wasHit)
            return;
        currentHealth = 0;
        ShootHitScan();
    }

    public void StartDissolver()
    {
        StartCoroutine(dissolver());
    }

    public IEnumerator dissolver()//CAN DO MULTIPLE WITH GO ARRAY
    {
        
        float elapsedTime = 0;
        Material dissolveMaterial = GetComponent<Renderer>().material;
        this.GetComponent<Collider>().enabled= false;
        while(elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(0,1, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveStrength", dissolveStrength);
            

            yield return null;
        }
        //potentially add disable collider functionality. 
    }


    //Functions not being used
    /* private void StopSpawn()
    {
        for(int i =0; i<spawnPoints.Length;i++)
        {
            spawnPoints[i].SetActive(false);
        }
    } */
}
