using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBox : MonoBehaviour
{
    [Header("All objects need these")]
    public AudioSource Beep;
    public AudioClip soundClip;
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
    float timeOfMove = 2; 

     //Move Vars
    private float startTime;
    private float journeyLength;
    private bool movingActivated;


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
    public bool puzzleComplete;//Might need it? 

    //Saving variables
    public delegate void PuzzleCompleted(int num);
    #pragma warning disable 67
    public event PuzzleCompleted OnPuzzleFinish;
    #pragma warning restore 67
    [HideInInspector]
    public bool puzzleDone;
    [HideInInspector]
    public int puzzleIndex;

    public void ActionPressed()
    {
        
        activateCount++;
        if(activateCount == amountToActivate)
        {
            Debug.Log("DO DA THING PLEASEEEE");
                if(activate)
                {
                    ActivateObject();
                }
                if(deactivate)
                {
                    DeactivateObject();
                }
                else if(dissolveOBJ)
                    StartCoroutine(dissolver()); 
                if(move)
                {
                    Move();
                }
                if(spawn)
                {
                    SpawnObject();
                }
        }
    }
    private void Move()
    {
        if(effectForGod!=null)
        {
            Instantiate(effectForGod,transform.position, Quaternion.identity);
        }
        if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
        

        /* movingActivated = true;
        puzzleComplete = true; */
        StartCoroutine(MoveObject(endPosition, timeOfMove));
    }

    private IEnumerator MoveObject(Vector3 targetPosition, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector3 currentPosition = transform.position;

        while (Time.time < endTime)
        {
            float journeyFraction = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(currentPosition, targetPosition, journeyFraction);

            yield return new WaitForFixedUpdate(); 
        }
        transform.position = targetPosition;
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
    
}
