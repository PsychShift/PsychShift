using System;
using System.Collections;
using System.Collections.Generic;
using Guns.Health;
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
    [HideInInspector]
    public int index;

    
    //Actions
    [Header("How do you wanna interact?")]
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool pressurePlate;
    [SerializeField]
    private bool shootObj;
    [SerializeField]
    public bool leverPull;
    [Header("Needed for level animation, ONLY parent panel and audio clips")]
    public GameObject leverParent;
    /* private bool killEnemy;
    [Header("Add this if killing an enemy to activate")]
    [SerializeField]
    public EnemyHealth enemyTarget; */

    [Header("Add and select these for GOD obj")]
    public ParticleSystem effectForGod;
    [SerializeField]//Add this number on god box
    int amountToActivate;
    public int activateCount=0;

    [Header("What do you want god to do?(can do multiple)")]
    [Header("Move options")]
    [SerializeField]
    private bool move;
    [SerializeField]
    private bool isAnim;
    [SerializeField]
    private string animVarName;
    
    [SerializeField] private Vector3 endPosition;

    [SerializeField]
    float timeOfMove = 2; 


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
    [Header("USE THIS IF ACT IS SEPERATE FROM OBJ YOU INTERACT WITH")]
    public bool doNotReact;
    

    [Header("THIS IS FOR CHANGING BUTTON COLOR AND STUFF WHEN ACTIVATED")]
    [SerializeField]
    private bool changeObject;
    [Tooltip("GO is what it turns into")]
    [SerializeField]
    GameObject activatedObject;//WHAT WAS I THINKING AAAAAAAAAAAAAAAAAAAAAAAAAA



    //these are in script only
    private bool godBox;
    [HideInInspector]
    public bool activated;
    public bool puzzleComplete;

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}

    public int MaxHealth { get { return 1; } }

    public bool IsWeakPoint { get; } = false;

    private bool destructObject;

    
    //Move Vars
    private float startTime;
    private float journeyLength;
    private bool movingActivated;

    //Saving variables
    public delegate void PuzzleCompleted(int num);
    public event PuzzleCompleted OnActivated;
    [HideInInspector]
    public bool puzzleDone;
    [HideInInspector]
    public int puzzleIndex;


    //VARIABLE THAT NEED ORGANIZING
    private void Awake() 
    {
        if(godBoxRef!= null)
        {
            godBox = false;
            if(effectForAct== null)
            {
                //Debug.LogError("UH OH NO EFFECTS FOR THIS ACTION OBJ DETECTED LUL(HINT IM ONE OF THE PUZZLE KIT OBJ, I'M NOT THE GOD ONE THO) "+ gameObject.name);
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
                //Debug.LogError("UH OH YOU FORGOT GOD BOX EFFECT LUL: "+ gameObject.name);
            } 
        }
        CurrentHealth = MaxHealth;
        if(changeObject == false && doNotReact == false )
            destructObject = true;
       /*  if(killEnemy == true)
        {
            enemyTarget.OnDeath += ThisActivate;
        } */
        

    }
    //public void Update()
    //{
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

        /* if(movingActivated && godBox)
        {
            
            Vector3.Lerp(transform.position, endPosition, speedOfMove);
            float distCovered = (Time.time - startTime) * speedOfMove;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, endPosition, fracJourney);
            if(fracJourney >= 1)
            {
                movingActivated = false;
            }
            
        } */
        /* if(puzzleTimeBegan)
        {

        } */
    //}

    private IEnumerator MoveObject(Vector3 targetPosition, float duration)
    {
        //CanInteract = false;
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector3 currentPosition = transform.position;

        while (Time.time < endTime)
        {
            float journeyFraction = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(currentPosition, targetPosition, journeyFraction);

            /* if (collisionDetection != null)
            {
                Collider[] colliders = Physics.OverlapBox(collisionDetection.position, collisionDetectionSize / 2);
                foreach(Collider other in colliders)
                {
                    if(other.tag == "Destructable")
                    {
                        if(other.TryGetComponent(out IDamageable damageable))
                        {
                            damageable.TakeDamage(999);
                        }
                    }
                }
            } */
            yield return new WaitForFixedUpdate(); 
        }
        transform.position = targetPosition;
        //CanInteract = true;
    }
    /* IEnumerator MoveFunct()
    {
                Vector3.Lerp(transform.position, endPosition, speedOfMove);
                float distCovered = (Time.time - startTime) * speedOfMove;
                float fracJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(transform.position, endPosition, fracJourney);
                if(fracJourney >= 1)
                {
                    movingActivated = false;
                }
            
            if(movingActivated == true)
                StartCoroutine(MoveFunct());
            else
                yield return null;
    } */


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
        if(isAnim== false)
            StartCoroutine(MoveObject(endPosition, timeOfMove));
        //StartCoroutine(MoveFunct());
        /* while(movingActivated == true)
        {
            Vector3.Lerp(transform.position, endPosition, speedOfMove);
            float distCovered = (Time.time - startTime) * speedOfMove;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, endPosition, fracJourney);
            if(fracJourney >= 1)
            {
                movingActivated = false;
            }
        } */
                
        #region 
        
        if(isAnim)
        {
            //PLAY animation here
            Debug.Log("Trying ANim");
            Animator godBoxAnim = gameObject.GetComponent<Animator>();
            godBoxAnim.SetBool(animVarName, true);
        }
         //GARBO CODE that I'm hoarding
        
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
        
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        for(int i = 0; i< activateObject.Length;i++)
        {
            if(effectForGod!=null)
            {
                Instantiate(effectForGod,activateObject[i].transform.position, Quaternion.identity);
            }
            activateObject[i].SetActive(true);
        }
        
            
        
    }

    private void DeactivateObject()
    {
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        for(int i = 0; i< deactivateObject.Length;i++)
        {
            if(effectForGod!=null)
            {
                Instantiate(effectForGod,activateObject[i].transform.position, Quaternion.identity);
            }
            deactivateObject[i].SetActive(false);
        } 
    }
    

    public virtual void ThisActivate()
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
                OnActivated?.Invoke(index);
                if(soundClip!=null)
                    Beep.PlayOneShot(soundClip);
                if(effectForAct!=null)  
                    Instantiate(effectForAct,transform.position, Quaternion.identity); 
                Collider turnOff=gameObject.GetComponent<Collider>();
                if(turnOff!=null)
                    turnOff.enabled = false;//moved this from below gBoxRef
                godBoxRef.activateCount++;
                
                Debug.Log("Activated: " + godBoxRef.activateCount);
                godBoxRef.ThisActivate();
                //Destroy(this.gameObject);
                if(changeObject)
                {
                    GetComponent<Collider>().enabled= false;
                    GetComponent<MeshRenderer>().enabled = false;
                    
                    activatedObject.SetActive(true);
                    this.gameObject.SetActive(false); 
                }
                    
                    //this.gameObject.SetActive(false);  
                else if(destructObject)
                {
                    Collider collider = GetComponent<Collider>();
                    //GetComponent<Collider>().enabled= false;
                    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                    //GetComponent<MeshRenderer>().enabled = false;
                    if(collider!= null)
                        collider.enabled = false;
                    if(meshRenderer!= null)
                        meshRenderer.enabled = false;
                    else
                        Destroy(gameObject);
                }
                else if(doNotReact)
                {
                    //Does nothing to action object since the object is an empty game object//Usually used for killing enemies
                }
                
                    //Destroy(gameObject);
                /* else if(dissolveOBJ)
                    StartDissolver(); */
                

                    

                //Change color or object
            }
        }
        else if(godBox == true)
        {
            if(activateCount == amountToActivate || puzzleDone == true)//runs when called to check if everything is activated
            {
                puzzleDone = true;
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
        else if(leverPull)
        {
            /* if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                LeverAction();
            } */
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
    public void TakeDamage(int Damage, Guns.GunType gunType)
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
    public void ThisActivate(Transform transform)
    {
        //enemyTarget.OnDeath-=ThisActivate;
        ThisActivate();
    }
    private void OnDisable() 
    {
        /* if(enemyTarget!=null)
            enemyTarget.OnDeath -= ThisActivate;   */  
    }
    private void LeverAction()
    {
        //Turn off panel
        //swap to camera/panel objects
        //play coroutine
        //When done turn back on other panel
        //turn on green button 
        //Before -> cutscene -> after/greenbutton
        //leverChildren = leverParent.transform.GetChild();
    }


    //Functions not being used
    /* private void StopSpawn()
    {
        for(int i =0; i<spawnPoints.Length;i++)
        {
            spawnPoints[i].SetActive(false);
        }
    } */
    //Get a ref to the checkpoint related to the encounter
    //Call this function to write the current objective
    //New function that keeps track of what has been activated. Possibly ask for new ref in activate function that needs action ref. 
}
