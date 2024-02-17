using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBox : MonoBehaviour, IDamageable
{
    [Header("All objects need these")]
    public AudioSource Beep;
    public AudioClip soundClip;
    //Actions
    [Header("How do you wanna interact?")]
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool pressurePlate;
    [SerializeField]
    private bool shootObj;
    [Header("Add and select these for action obj")]
    [SerializeField]
    GodBox godBoxRef;
    public ParticleSystem effectForAct;
    [Header("THIS IS FOR CHANGING BUTTON COLOR AND STUFF WHEN ACTIVATED")]
    [SerializeField]
    private bool changeObject;
    [Tooltip("GO is what it turns into")]
    [SerializeField]
    GameObject activatedObject;
    

     public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}

    public int MaxHealth { get { return 1; } }

    public bool IsWeakPoint { get; } = false;

    private bool destructObject;
    private bool activated;

    private void Awake() {
        CurrentHealth = MaxHealth;
        if(changeObject == false)
            destructObject = true;
    }
    public void ThisActivate()
    {
        //activates something when called
        //check if activated already
        //add to counter
        //Call this whenever an item is interacted with. NEEDS REF TO PUZZLE KIT   
            
                activated = true;
                if(soundClip!=null)
                    Beep.PlayOneShot(soundClip);
                if(effectForAct!=null)  
                    Instantiate(effectForAct,transform.position, Quaternion.identity); 
                Collider turnOff=gameObject.GetComponent<Collider>();
                turnOff.enabled = false;
                godBoxRef.ActionPressed();
                //Destroy(this.gameObject);
                if(changeObject)
                {
                    activatedObject.SetActive(true);
                    this.gameObject.SetActive(false);  
                }
                else if(destructObject)
                    Destroy(this.gameObject);
                //godBoxRef.ActionPressed();
                //Change color or object
            
        
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
    private bool wasHit => CurrentHealth == 0;
    public void TakeDamage(int Damage)
    {
        if(wasHit)
            return;
        currentHealth = 0;
        ShootHitScan();
    }
}
