using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guns.Health;
using Guns;
using System;
public class EnemySpawner : MonoBehaviour
{
    private static System.Random rand = new System.Random();
    [SerializeField]
    PuzzleKit godBoxRef;
    [SerializeField] private GameObject enemyPrefab;
    
    //[SerializeField] GameObject[] enemyType;

    [SerializeField] private List<EBrainType> enemyTypes;
    [SerializeField] private List<GunScriptableObject> gunTypes;
    [SerializeField] private EnemyModifierDictionary enemyModifiers;
    [SerializeField] private List<AIAgression> agressionLevels;

    //[SerializeField] bool randomEnemySpawn;
    //[SerializeField] int whatEnemytoSpawn;//select int to only spawn an enemy of that type

    [SerializeField] int howManySpawn;
    [SerializeField] float spawnRate;
    private List<GameObject> enemySpawned;

    
    [SerializeField] Vector2 gapBetweenSpawns;
    [Header("TURN THIS OFF MANUALLY FOR KILL EVERYONE SECTIONS")]
    public bool puzzleNotDone= true;
    public PuzzleKit godBoxActionRef;
    int deathCount=0;
    public ParticleSystem spawnFX;

    private void Start() 
    {
        enemySpawned = new();
        StartCoroutine(SpawnIn());
    }

    private IEnumerator SpawnIn()
    {
        /* wait for respawn check, if youre down enemies respawn, else wait again to check
        pick from the lists :
            enemyTypes
            gunTypes
            enemyModifier
        instantiate the enemy prefab
        get the EnemyBrainSelector component 
        call swap brain
        */

        
        yield return new WaitForSeconds(spawnRate);
        int numToSpawn = Math.Abs(enemySpawned.Count - howManySpawn);

        for(int i = 0; i < numToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            // Choose enemy type stuff
            EBrainType brain = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)];

            GunScriptableObject gun = gunTypes[UnityEngine.Random.Range(0, gunTypes.Count)];

            List<EEnemyModifier> selectedModifiers = new();
            foreach(var mod in enemyModifiers)
            {
                float chance = mod.Value / 100f;
                if(rand.NextDouble() < chance)
                {
                    selectedModifiers.Add(mod.Key);
                }
            }

            AIAgression agression = agressionLevels[UnityEngine.Random.Range(0, agressionLevels.Count)];


            // Set enemy type
            EnemyBrainSelector selector = enemy.GetComponent<EnemyBrainSelector>();
            selector.SwapBrain(gun, brain, selectedModifiers, agression, true);

            // subscribe to death event
            enemy.GetComponent<EnemyHealth>().OnDeath += EnemyDeath;
            enemySpawned.Add(enemy);
            
            // Special effects on spawn
            Instantiate(spawnFX, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(UnityEngine.Random.Range(gapBetweenSpawns.x, gapBetweenSpawns.y));//gap between spawns 
        }
    } 
    
    /* private IEnumerator SpawnIn()
    {
        yield return new WaitForSeconds(spawnRate);
        for(int i = 0; i<howManySpawn; i++)
        {
                if(randomEnemySpawn)
                    enemySpawned.Add(Instantiate(enemyType[UnityEngine.Random.Range(0,enemyType.Length)], transform.position,Quaternion.identity));
                else 
                {
                   enemySpawned.Add(Instantiate(enemyType[whatEnemytoSpawn], transform.position,Quaternion.identity)); 
                }
                Instantiate(spawnFX,transform.position, Quaternion.identity);
                enemySpawned[enemySpawned.Count-1].GetComponent<EnemyHealth>().OnDeath += EnemyDeath;
                //PuzzleKit.PuzzleDone+=PuzzleFinished;
                
        } 
    } */
    
    private void EnemyDeath(Transform enemTransform)
    {
        //Debug.Log("ACTIVATED");
        if(godBoxRef!= null && godBoxRef.puzzleComplete)
                puzzleNotDone = false;
        if(puzzleNotDone == false)
        {
            if(enemySpawned.Count-2<=0)//WAS ONE MADE -2 FOR TEMP FIX
            {
                godBoxActionRef.activateCount++;
                
                godBoxActionRef.ThisActivate();
            }
            
        }
        enemTransform.GetComponent<EnemyHealth>().OnDeath -= EnemyDeath;
        enemySpawned.Remove(enemTransform.gameObject);
        if(puzzleNotDone && enemySpawned.Count-2<=0)//WAS ONE MADE -2 FOR TEMP FIX 
        {
            StartCoroutine(SpawnIn());
            
        }
    }
    void OnDestroy()
    {
        foreach(var enemy in enemySpawned)
        {
            enemy.GetComponent<EnemyHealth>().OnDeath -= EnemyDeath;
        }
        enemySpawned.Clear();
    }
    private void PuzzleFinished()
    {
        puzzleNotDone = false;
    }
    //[SerializeField]
    //bool elimEnemies;
    /* [SerializeField]
    bool waveSpawn; */
    //private void Update()
    //{
        //Choose what enemy to spawn
        
        /* if(elimEnemies)
        {
            if(enemySpawned.Count<=0)
            {
                SpawnIn();
            }
        }
        else if(waveSpawn)
        {
            
        } */
            
    //}
    /* private IEnumerator Spawnwait()
    {
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnIn());
    } */
    /* IEnumerator spawnWave()
    {
        
        while(puzzleNotDone && enemySpawned.Count>0)
        {
            SpawnIn();
            yield return new WaitForSeconds(spawnRate);
        }
        
    } */
}

[Serializable]
public class EnemyModifierDictionary : SerializableDictionary<EEnemyModifier, int> { }
